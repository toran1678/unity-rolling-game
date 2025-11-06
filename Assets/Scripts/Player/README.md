# RollingCircleController 사용 가이드

## 개요
원 스프라이트가 굴러다니며 이동하는 효과를 구현한 컨트롤러입니다.

## 설정 방법

### 1. 게임 오브젝트 설정
1. Unity에서 빈 GameObject를 생성하거나 기존 GameObject를 선택합니다
2. **SpriteRenderer** 컴포넌트를 추가하고 원 스프라이트를 할당합니다
3. **Rigidbody2D** 컴포넌트를 추가합니다
   - Body Type: Dynamic
   - Gravity Scale: 0 (2D 플랫포머가 아닌 경우)
   - Freeze Rotation Z: 체크 (물리 회전 방지, 스크립트로 회전 제어)
4. **RollingCircleController** 스크립트를 추가합니다

### 2. Input System 설정

#### 방법 1: 자동 할당 (권장)
- 스크립트가 자동으로 `Assets/InputSystem_Actions.inputactions` 파일을 찾습니다
- **별도 할당 없이 바로 사용 가능합니다!**

#### 방법 2: 수동 할당
1. Unity 에디터에서 게임 오브젝트를 선택합니다
2. Inspector에서 **RollingCircleController** 컴포넌트를 찾습니다
3. **"참조"** 섹션의 **"Input Actions"** 필드를 찾습니다
4. Project 창에서 `InputSystem_Actions` 파일을 찾습니다
5. `InputSystem_Actions` 파일을 **Input Actions 필드로 드래그 앤 드롭**합니다

**참고**: `InputSystem_Actions.inputactions` 파일은 `Assets` 폴더에 이미 있습니다.

### 3. 파라미터 조정

#### 이동 설정
- **Move Speed**: 이동 속도 (기본값: 5)
- **Acceleration**: 가속도 (기본값: 10)
- **Deceleration**: 감속도 (기본값: 10)

#### 회전 설정
- **Circle Radius**: 원의 반지름 (스프라이트 크기에 맞게 조정)
- **Use Sprite Renderer For Radius**: 체크 시 스프라이트 크기에서 자동으로 반지름 계산

## 작동 원리

1. **이동**: Input System의 "Move" 액션을 읽어서 Rigidbody2D로 이동합니다
2. **회전**: 이동 거리를 계산하여 원의 둘레에 비례하여 회전시킵니다
   - 공식: 회전 각도 = (이동 거리 / 원의 둘레) × 360도
   - 오른쪽 이동: 시계 방향 회전
   - 왼쪽 이동: 반시계 방향 회전

## 조작 방법

- **WASD** 또는 **화살표 키**: 이동
- **게임패드 왼쪽 스틱**: 이동

## 문제 해결

### 원이 제대로 굴러다니지 않을 때
1. **Circle Radius** 값을 스프라이트의 실제 반지름에 맞게 조정하세요
2. **Use Sprite Renderer For Radius**를 체크하여 자동 계산을 사용하세요
3. Rigidbody2D의 **Freeze Rotation Z**가 체크되어 있는지 확인하세요

### 이동이 안 될 때
1. Input Actions 에셋이 제대로 할당되었는지 확인하세요
2. Input System 패키지가 설치되어 있는지 확인하세요
3. Rigidbody2D의 **Body Type**이 Dynamic인지 확인하세요

## 추가 기능

스크립트는 다음 메서드를 제공합니다:
- `SetMoveSpeed(float speed)`: 이동 속도 변경
- `SetCircleRadius(float radius)`: 원의 반지름 변경
- `GetVelocity()`: 현재 속도 반환
- `GetTotalDistanceTraveled()`: 총 이동 거리 반환

