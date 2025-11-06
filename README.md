# Unity Project - Collaboration README

본 문서는 협업을 위한 공통 규칙과 `Assets/` 폴더 구조, 작업 절차를 정의합니다. 새로운 기여자는 이 문서를 먼저 읽고 환경을 세팅하세요.

## 필수 정보
- **Unity 버전**: 6000.2.10f1

## Assets 폴더 구조 (요약)
현재 리포지토리의 주요 구조입니다. 새 폴더 추가 시 아래 규칙을 따르세요.

```
Assets/
  Audio/                               # 사운드 에셋 (BGM, SFX)
  Fonts/                               # 폰트 파일
  Materials/                           # 공용 머티리얼
  Prefabs/                             # 프리팹 (재사용 단위)
  Scenes/                              # 최상위 씬 모음
    SampleScene.unity
  Scripts/                             # 스크립트 루트
    Player/                            # 기능/도메인 단위 하위 폴더
      RollingCircleController.cs
      README.md
  Settings/                            # URP, 템플릿 등 프로젝트 설정 자산
    Scenes/                            # 씬 템플릿 리소스
  Sprites/                             # 2D 스프라이트/텍스처
  UniversalRenderPipelineGlobalSettings.asset
  DefaultVolumeProfile.asset
  InputSystem_Actions.inputactions
```

## 폴더 규칙
- **기능/도메인 우선 분류**: `Scripts/Player`, `Scripts/AI`, `Scripts/UI`처럼 도메인 중심으로 폴더링
- **에셋-스크립트 근접 배치 지양**: 에셋은 `Audio/`, `Sprites/`, `Materials/`, 모델은 `Models/`(필요시)로 일원화
- **씬**: 최상위는 생산용 씬만. 실험/개발용은 `Scenes/_Sandbox/` 하위에 본인 이름/이슈번호로 분리 예) `Scenes/_Sandbox/park-1234.unity`
- **프리팹**: 공용은 `Prefabs/`, 특정 도메인 전용은 `Prefabs/Player/` 등 하위 폴더 생성
- **Settings**: 렌더러/파이프라인/볼륨/템플릿 등 프로젝트 설정 에셋만 보관

## 네이밍 컨벤션
- **파일/폴더**: PascalCase (예: `RollingCircleController.cs`, `PlayerController.prefab`)
- **프리팹**: `P_` 접두어 선택적 (예: `P_Player`) — 팀 내 합의 후 일괄
- **스크립트 클래스**: 파일명 == 클래스명, PascalCase
- **애니메이션/타임라인**: `Target_Action_Variant` (예: `Player_Run_Fast`)
- **Input Action**: 맥락/행동 기준 (예: `Player/Move`, `Player/Jump`)

## 버전 관리(Git)
- **.meta 필수**: Unity는 GUID 기반 참조. `.meta` 포함 커밋 필수
- **브랜치 전략**: `main`(안정) / `develop`(통합) / `feature/<topic>` / `hotfix/<issue>`
- **커밋 메시지**: 한글 OK. 타입 권장: `feat:`, `fix:`, `refactor:`, `docs:`, `chore:`, `perf:`
- **대용량**: 바이너리(에셋) 변경은 PR 설명에 변경 범위/용량 기재

## 작업 절차
1) 이슈 생성 → 범위, 수락 기준 정의
2) 브랜치 필수 생성·이동: `feature/<이슈-키워드>` 또는 `hotfix/<이슈-번호>`
   - 현재 브랜치 확인: `git branch`
   - 브랜치 생성/체크아웃: `git checkout -b feature/<이슈-키워드>`
3) 구현
   - 스크립트: `Scripts/<Domain>/...`
   - 에셋: 해당 카테고리 폴더에 저장
   - 씬 작업은 Sandbox에서 검증 후 메인 씬 반영
   - 자체 점검 체크리스트
     - 콘솔 에러/경고 0 확인
     - 누락된 참조/Prefab Apply 여부 확인
     - 플레이/정지 반복 시 상태 누수 없는지 확인
4) 원격 동일 브랜치로 푸시 (필수)
   - 최초 푸시: `git push -u origin feature/<이슈-키워드>`
   - 이후 푸시: `git push`
5) PR 생성 (필수)
   - 대상 브랜치: `develop`(또는 지정된 기본 브랜치)
6) 병합
   - 승인 후 팀 규칙에 따라 Squash/Merge 진행
   - 로컬 정리: `git checkout develop && git pull` (필요 시 작업 브랜치 삭제)

주의: `main`/`develop`에 직접 푸시 금지. 모든 변경은 개인 브랜치 → PR → 병합 절차를 따릅니다.
