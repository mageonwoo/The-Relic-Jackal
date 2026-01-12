# Relic Jackal

개인 게임 프로젝트\
A* 기반 의사결정 구조 실습 프로젝트

---

## Project Summary

**Relic Jackal**은 턴 기반 구조 안에서\
이동 → 선택 → 전투가 연결되는 흐름을 구현하기 위한 프로젝트입니다.

A* 알고리즘을 단순 이동이 아닌 의사결정 근거로 사용하는 것을 목표로 하고 있으며,\
현재는 전체 게임이 아닌 핵심 시스템 파트(Vertical Slice) 를 구현 중입니다.

---

## Key Features (현재 구현된 부분)

- Grid 기반 보드 이동
- A* 기반 경로 계산
- 이동 가능 지점 표시 (Reachable Path Visualization)
- A* 힌트 경로 시각화 (Hint Path Visualization)

---

## Technical Stack

### Engine
Unity
### Language
C#
### Pattern 적용 영역:
- Data / Calculation / Rendering 분리
- 순수 계산 객체(Pure Calculator) 적용
- MonoBehaviour 의존 최소화 (board/renderer 분리)

---

## System Details
### 1. A* Pathfinding 적용

맵은 Grid 기반이며\
플레이어 이동 시 다음을 수행합니다:
- 이동 가능 지점 계산
- 목표 지점까지의 A* 경로 계산
- 경로 길이에 따른 위험 비용 반영(실험)

A*는 길찾기 역할 외에\
**선택 근거(Decision Input)** 로 활용하는 방향을 시도 중입니다.

### 2. Phase 분리 구조

전투는 단일 처리 대신 Phase 로 분리합니다:
- 이동 / 탐색 Phase
- 전투 Phase
- 결과 Phase

이 방식은 이후에
AI/몬스터/자원 노출 등의 시스템이 연결될 수 있는 구조를 목표로 합니다.

### 3. 선택과 결과 흐름 (Prototype)

플레이어의 선택에 항상 결과가 존재하도록 설계합니다:
- 위험한 선택 → 손해/전투 발생 확률 증가
- 안전한 선택 → 진행 안정적/속도 낮음

정확한 밸런스보다는
**Event → Result 흐름**을 만드는 경험에 초점이 있습니다.

---

## Preview
### Optimal Path Demo
![Path Demo](https://github.com/user-attachments/assets/d61ba2b5-7dc8-4298-a939-03bd19cb339e)
### Hint & Reachable Demo
![Hint Demo](https://github.com/user-attachments/assets/03aa3557-d4a4-4599-a3f8-c6d9cc39d090)

---

## Project Status

현재 진행 상태:

- [x] Grid 기반 맵 구성
- [x] A* Pathfinding 계산기 구현
- [x] 이동 가능 범위 시각화
- [x] 경로 힌트 시각화
- [ ] 규칙 객체 설계 (~70%)
- [ ] 턴/전투 Phase 통합
- [ ] 결과 처리(Outcome) 구조 적용

---

## Repository Structure

Assets\
├─ 01.Core     (계산/순수로직)\
├─ 02.Data     (ScriptableObject, Config)\
├─ 03.Gameplay (Controller 및 Phase 제어)\
└─ 99.Dev      (테스트용 UI/도구)

구조는 계산 / 데이터 / 연출(렌더링) 을 분리하는 것을 목표로 합니다.

## Author

### 마건우
Unity(C#) 기반 게임플레이/클라이언트 개발 학습 중
