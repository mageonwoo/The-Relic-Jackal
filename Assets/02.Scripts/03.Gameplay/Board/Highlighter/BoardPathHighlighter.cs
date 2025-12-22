using System.Collections.Generic;
using UnityEngine;

// 스크립트가 하는 일
// 1. path좌표에 하이라이트 오브젝트를 배치해 보이게 한다.
// 2. 다음 경로를 보기 전에 이전 하이라이트를 끈다.
// 3. 이 스크립트는 경로를 계산하지 않는다.
// 4. 계산은 A*, Highlighter는 그 결과를 보여준다.
namespace Gameplay.Board.Highlighter
{
    public class BoardPathHighlighter : MonoBehaviour
    {
        // 하이라이트 오브젝트
        // 각자 선언 한 것이 아니라 구조체 단위로 선언
        [System.Serializable]
        private sealed class HLLayerConfig
        {
            public HLLayer layer;
            public GameObject pathPrefab;
            public float yOffset;
        }
        [Header("PathLayer Prefab array")]
        [SerializeField] private HLLayerConfig[] configs;

        // 1. 내가 찍어 놓은 하이라이트를 전부 Clear를 통해 전부 지우기 위한 리스트
        //    리스트에 저장하지 않으면 뭘 지워야 할 지 모름. (개념 이해할 것)
        // ↓
        // 2. Instantiate → Destroy에서 ObjectPool로 교체
        //    List의 이름을 _spawned → _pool로 교체
        // ↓
        // 3. Layer 구조체의 멤버변수별 Dictionary화
        //    activeCount도 Layer별 파악을 위해 Map에 저장

        // 레이어 속성
        private readonly Dictionary<HLLayer, HLLayerConfig> _configMap = new();
        // 레이어 pool리스트
        private readonly Dictionary<HLLayer, List<GameObject>> _poolMap = new();
        // 레이어 오브젝트 사용 개수
        private readonly Dictionary<HLLayer, int> _activeCountMap = new();

        void Awake()
        {
            _configMap.Clear();
            _poolMap.Clear();
            _activeCountMap.Clear();

            //LayerConfig가 없으면 그냥 리턴
            if (configs == null) return;

            for (int i = 0; i < configs.Length; i++)
            {
                HLLayerConfig lc = configs[i];
                if (lc == null || lc.pathPrefab == null) continue;

                _configMap[lc.layer] = lc;
                _poolMap[lc.layer] = new List<GameObject>();
                _activeCountMap[lc.layer] = 0;
            }
        }

        // Clear를 통해 리스트의 모든 하이라이트 프리팹을 반복문을 통해 파괴
        // → 오브젝트를 파괴하지 않고 비활성화만 진행
        // ↓
        // 지정한 레이어에서 사용한 하리아이트 오브젝트를 전부 끄고, 사용 개수를 0으로 되돌린다.
        ///////////////////////////////////////////////////////////////////////////
        // 이번에 활성화된 오브젝트만 비활성화하고 activeCount를 리셋해 재사용을 준비한다. //
        ///////////////////////////////////////////////////////////////////////////
        public void Clear(HLLayer layer)
        {
            // layer에 해당하는 풀(List<GameObject>)이 있는지 확인하고 없으면 바로 리턴
            // 관리 대상이 아니면 무시한다.
            // 등록 되지 않은 레이어를 Clear하다 NRE터지는 것을 방지
            if (_poolMap.TryGetValue(layer, out var pool) == false) return;

            // 이번 ShowCells 호출에서 실제로 켜진 오브젝트 개수가 없으면 바로 리턴
            if (_activeCountMap.TryGetValue(layer, out int activeCount) == false) return;

            // pool[0] ~ pool[activeCount - 1]에서 사용한 오브젝트 들만 비활성화
            // pool.Count가 아니라 activeCount만 종료.
            for (int i = 0; i < activeCount; i++)
                pool[i].SetActive(false);

            // 지정한 레이어에서 사용 중인 오브젝트가 없으므로,
            // 다음 ShowCells 호출 시 pool[0]부터 다시 사용하겠다는 선언
            _activeCountMap[layer] = 0;
        }

        // 딕셔너리 작성 후 추가
        // 등록된 모든 레이어에 대해 Clear(Layer)를 한 번씩 호출 한다.
        public void ClearAll()
        {
            foreach (var kv in _poolMap)
                Clear(kv.Key);
        }

        // BoardManager: 좌표 → worldPos 위치로 변환하기 위함
        // Vector2Int List: A*가 계산한 경로 좌표 리스트
        // 받아서 그리기만 하는 매서드
        public void ShowCells(BoardManager board, List<Vector2Int> cells, HLLayer layer)
        {
            // 입력을 방어
            if (board == null || board.grid == null)
            {
                Debug.LogError("PathHighlighter: board 누락");
                return;
            }
            if (_configMap.TryGetValue(layer, out var config) == false || config.pathPrefab == null)
            {
                Debug.LogError("PathHighlighter: config 누락");
                return;
            }

            // 새로 그리기 전에 해당 레이어를 삭제 → 갱신.
            Clear(layer);

            if (cells == null || cells.Count == 0) return;

            // 경로 칸 개수 만큼 반복 → 좌표마다 표시
            for (int i = 0; i < cells.Count; i++)
            {
                // 보드의 좌표 유효성 검사.
                Vector2Int p = cells[i];
                if (p.x < 0 || p.y < 0 || p.x >= board.width || p.y >= board.height)
                    continue;

                // // 보드 좌표 -> worldPos로 변환
                // // 여기가 제일 중요함.
                GameObject go = GetOrCreate(layer, config.pathPrefab);
                // _activeCount++;
                go.SetActive(true);
                go.transform.position = board.grid[p.x, p.y].worldPos + Vector3.up * config.yOffset;
            }
        }

        // 레이어별 오브젝트 풀에서 재사용하거나, 부족할 때 생성해서 리스트에 추가.
        // layer = 어느 레이어 풀을 쓸것인가
        // pool = 이 레이어의 전용 리스트
        // Object Pool의 생명주기를 관리하는 매서드 (책임 분리)
        private GameObject GetOrCreate(HLLayer layer, GameObject prefab)
        {
            List<GameObject> pool = _poolMap[layer];
            int activeCount = _activeCountMap[layer];

            if (activeCount < pool.Count)
            {
                _activeCountMap[layer] = activeCount + 1;
                return pool[activeCount];
            }

            GameObject go = Instantiate(prefab);
            pool.Add(go);
            _activeCountMap[layer] = activeCount + 1;
            return go;
        }
    }
}