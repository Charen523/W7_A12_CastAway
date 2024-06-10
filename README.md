<br/>
<br/>

# <p align="center"> **W7 A12  Cast Away : Island Exodus**  </p>

##### <p align="center"> <b> 내일 배움 캠프 7주차 팀프로젝트 : Team.유록과 12조 제자들 </b>

<br/>

![startScene](https://github.com/Charen523/W7_A12_CastAway/assets/108499207/44192b3e-4af3-4c98-9a89-62482a7faeae)

<br/>
<br/>

<br/>

---

### 📖 목차
+ [자기 소개](#자기-소개)
+ [프로젝트 소개](#프로젝트-소개)
+ [깃 컨벤션](#깃-컨벤션)
+ [기능 소개](#기능-소개)
+ [트러블슈팅](#트러블슈팅)
+ [작업 타임라인](#작업-타임라인)
+ [시연 영상 링크](#시연-영상-링크)

---

### ✨자기 소개
| 이름   | 직책 | 직무 |
|--------|------|------|
| 정승연 | 팀장 | 프로젝트 총괄제작 |
| 이정빈 | 팀원 | 프로젝트 총괄제작 |
| 이유신 | 팀원 | 프로젝트 총괄제작 |
| 김보근 | 팀원 | 프로젝트 총괄제작 |


---

### ✨프로젝트 소개

 `Info` **3D 서바이벌 게임**

 `Stack` C#, Unity-2022.3.17f, Visual Studio2022-17.9.6   

 `Made by` **정승연, 이정빈, 이유신, 김보근** 

 `Key Guide`


![KeyGuide](https://github.com/Charen523/W7_A12_CastAway/assets/108499207/ebb63cd9-3db6-4bda-9de0-39b8337a2f8d)

---

### ✨깃 컨벤션

- commit 규칙
    - 형식: [commit타입] 간단 설명

- commit 타입
    - init: 최초 커밋: Unity 프로젝트 생성
    - feat: 기능 추가
    - refactor: 기능 개선
    - add: 에셋, .cs 등 파일 추가
    - move: 파일 이동, 코드 이동 등
    - remove: 파일 삭제
    - art: UI 변경
    - fix: 버그 수정
    - chore: 기타 잡일
    - docs: 리드미 등 문서 수정
 
- branch
    - main
    - 형식: (브랜치 용도)/(상세)
    - 브랜치 이름규칙: commit 규칙과 같음.
---

### ✨기능 소개

- **필수 구현 사항:**
    - [ ]  **자원 수집 및 가공** (난이도: ★★★☆☆)
        - 플레이어가 자원을 찾아서 수집하고, 이를 가공하여 생존에 필요한 아이템을 제작할 수 있도록 합니다.
        - 자원 수집과 가공 메커니즘을 구현합니다.
    - [ ]  **식사와 수분 관리** (난이도: ★★☆☆☆)
        - 플레이어의 캐릭터가 식사와 수분을 관리해야 하며, 굶주림과 갈증을 방지해야 합니다.
        - 식사와 수분 관리 시스템을 구현하여 생존 요소를 추가합니다.
    - [ ]  **건축 및 생존 기지 구축** (난이도: ★★★☆☆)
        - 플레이어가 기지를 건축하고 안전한 곳을 만들 수 있도록 합니다.
        - 건축 및 기지 관리 메커니즘을 구현합니다.
    - [ ]  **적과의 전투** (난이도: ★★★☆☆)
        - 다양한 적과의 전투를 구현하고, 적의 AI를 제공하여 플레이어에게 도전을 제공합니다.
        - 전투 시스템과 AI를 개발합니다.
    - [ ]  **생존 관리 시스템** (난이도: ★★★☆☆)
        - 플레이어의 체력, 스태미너, 온도 등 생존과 관련된 상태를 관리합니다.
        - 생존 관리 시스템을 설계하고 구현합니다.
    - [ ]  **자원 리스폰** (난이도: ★★★☆☆)
        - 자원이 다시 생성되는 시스템을 구현하여 게임의 지속 가능성을 유지합니다.
        - 자원 리스폰 주기 및 메커니즘을 설계합니다.
- **선택 구현 사항:**
    - [ ]  **날씨와 환경 요소** (난이도: ★★★★☆)
        - 게임 세계의 날씨와 환경 요소가 플레이어의 생존에 영향을 미치도록 합니다.
        - 날씨 변화, 온도, 기상 조건 등을 추가합니다.
    - [ ]  **고급 건축 시스템** (난이도: ★★★★☆)
        - 다양한 건축 옵션과 고급 건축 요소를 추가하여 기지 건설을 더 다양하게 만듭니다.
        - 건축 및 구조물 개발 시스템을 확장합니다.
    - [ ]  **다양한 적 종류** (난이도: ★★★☆☆)
        - 다양한 종류의 적을 추가하여 게임의 다양성을 높입니다.
        - 다양한 동물과 적 캐릭터를 구현합니다.
    - [ ]  **크래프팅 시스템** (난이도: ★★★☆☆)
        - 다양한 아이템과 장비를 제작하기 위한 복잡한 크래프팅 시스템을 도입합니다.
        - 아이템 제작 및 조합 메커니즘을 확장합니다.
    - [ ]  **퀘스트와 스토리** (난이도: ★★★☆☆)
        - 게임에 퀘스트와 스토리 요소를 추가하여 게임 세계를 더 풍부하게 만듭니다.
        - 플레이어의 진행과 스토리 개발을 통합합니다.
    - [ ]  **고급 AI 시스템** (난이도: ★★★★☆)
        - 적과 동물의 AI를 더 복잡하게 만들어 게임의 난이도를 조절합니다.
        - AI 행동 패턴, 지능, 전략 등을 개발합니다.
    - [ ]  **사운드 및 음악** (난이도: ★☆☆☆☆)
        - 게임에 사운드 효과와 음악을 추가하여 게임의 분위기를 개선합니다.
        - 자연 소리, 적의 소리 효과 등을 통해 게임 환경을 풍부하게 만듭니다.
--- 

### ✨트러블슈팅


---

### ✨작업 타임라인

1일차 - 6월 3일.
프로젝트 Init + 각종 필요한 에셋 추가.
각자의 작업을 위해 씬 분리. (StartScene, MainScene, EntityScene)
정승연 - DayAndNight 추가.
정승연 - ReadMe 초안 작성.
이정빈 - Player관련 코드 초안 작성.(일부 강의코드 복붙.)
김보근 - 스타트씬 UI.
이유신 - 지형 정리(기존 Terrain 수정 작업) 및 맵 수정(접근 불가지역을 돌산으로 막기)
이정빈 - PlayerController 기본조작. (Condition.cs는 생략 예정 - 내가 리팩토링 다 다시함.)
정승연 - ItemSO, ConsumeSO, EquipSO 스크립트 작성. 및 SO 내용물 채우기.


2일차 - 6월 4일.
이정빈 - 캐릭터 시점 3인칭화.
정승연 - 아이템 프리팹과 SO를 ID로 관리하도록 ID규칙 생성 및 부여.
이유신 - 울타리, 동굴입구, 모래사장, 배 추가, 동굴 생성.
이유신 - 울타리 교체 스크립트 작성.
김보근 - AudioManager와 AudioMixerManager 만들고 StartScene 배경음악 추가
정승연 - 장비아이템 스크립트 구조화.(상속관계를 만들어둠.)
정승연 - Workshop UI 디자인 (가방, 장비, 제작)
이유신 - 오브젝트 풀을 통해 자원생성 구현.
이정빈 - 동물들 프리팹화
이유신 - 우물 오브젝트 디자인.
이유신 - 풀, 당근 배치 및 리젠
이유신 - 동굴에 길막용 바위 설치.
김보근 - ConditionUI 기본 디자인.
이정빈 - 동물AI 관련 스크립트 작성.
김보근 - UICondition 관련 스크립트 초안 작성.
정승연 - Workshop의 세 개 판넬 토글 완성.


3일차 - 6월 5일.
정승연 - 인벤토리 완전구현
정승연 - 태양 위치 변경
이정빈 - EntitySO 생성 및 연결
이유신 - 버섯 배치 및 리젠.
이유신 - 석탄, 철강 배치 및 리젠.
정승연 - 인벤토리와 플레이어 연결 및 Interaction 처음부터 재작성(호환이 안됨...)
*fix정승연 - interaction ray가 플레이어 오브젝트에 가로막히는 문제 해결.
이정빈 - Entity 스크립트 적용 및 AI 개선.
김보근 - Pause 버튼 생성.
이정빈 - 몬스터 공격모션 추가.


공휴일 - 6월 6일.
이유신 - 오브젝트 풀 스크립트 개선: 1스크립트 1오브젝트 - 1스크립트 N오브젝트.
이유신 - 자원 섞고 랜덤배치화.
정승연 - Controller에서 달리기 메커니즘 변경 및 Condition과 연결.
정승연 - alt키로 마우스 포인터 토글.
정승연 - Condition UI 리소스 이미지 새로 따와서 다시 제작.
정승연 - Condition 관련 코드들 리팩토링: Thirst, Temperature 만들고 필요 메서드 추가 및 UI와 Player 연결.
정승연 - Condition을 상속하는 Temperature 스크립트 작성 및 온도계UI.
정승연 - 소비템을 사용했을 떄 Condition에 반영되도록 리팩토링.
김보근 - 시간에 따라 플레이어의 온도가 바뀌는 스크립트 초안 작성.


4일차 - 6월 7일. (특강데이)
정승연 - tempUI 불러오는 UI 로직을 수정해 오류 방지.
이유신 - 자원 생성 범위 조정.
정승연 - EquipSO에 필요한 변수 추가&내용 작성. (장착될 위치, 장착시 늘려줄 스탯, 장착여부.)
이유신 - 집터 이동(호수에서 먼 곳으로, 이유: 게임성.)
정승연 - 인벤토리의 모든 버튼 작동테스트 완료.(4가지: 장착, 해제, 버리기, 사용.)
정승연 - 아이템 SO와 프리팹 위치를 Resources로 받아온 후 DataManager에서 관리하도록 함.
이정빈 - 장비 장착을 위한 기초 스크립트 작성.
*fix정승연 - Resources에서 폴더로 아이템 불러오는 코드 오류: 방법을 잘못 알고 있었음.
정승연 - index로 불러와서 모호하던 워크샵 판넬에 enum을 추가해 코드는 길어질 지언정 가독성 증가시킴.
이유신 - 당근, 버섯 아이템 상호작용.
정승연 - Condition에서 갈증 관련 메서드 추가: 수분 MaxValue 알려주기.(지금 생각해보니 어차피 필요가 없었...)
김보근 - 사망시 UI 완성.


주말 - 6월 8~9일.
이유신 - 호숫물 마시는 메커니즘 생성.
이정빈 - 공격 애니메이션 구현 및 장비 장착
이유신 - 우물물 마시는 메커니즘 생성.
이유신 - 집 계단, 내부 콜라이더 추가.
이유신 - 건축을 위한 BuildManager, BuildPrompt 추가.
이유신 - 우물에서 물통 아이템 획득.
이유신 - 상호작용을 통한 건축 완성.
이유신 - 호수 고갈 시 철조각 3개 나오도록 함.(일종의 이스터에그)
이유신 - 자원이 겹쳐 생성되지 않도록 오프셋 추가.
정승연 - 시간 UI 추가 + 게임매니저를 통해 게임시간 관리하도록 리팩토링.
정승연 - Pause판넬의 타임스케일 위치를 GM으로 이동.
정승연 - 온도 변화 메커니즘 재구성: 온도 변화시간을 시계 디자인과 동기화 및 빠르게 변화하도록 함.
정승연 - PausePanel 스크립트 작성. (기존 내용이 있었으나 밀어버리고 처음부터 다시 썼음.)
정승연 - PausePanelUI 제작. (기존 있었으나 밀고 다시만듦.)
정승연 - alt키 꾹 누르고 있으면 현재 Condition을 숫자로 확인할 수 있도록 함.
이유신 - 덤불 & 풀 채집 가능.
정승연 - 레시피 스크립트 구현.
정승연 - 제작대에서 아이템 제작 가능하도록 완성.
이유신 - 돌멩이, 나뭇가지 맵에 배치.
이유신 - 자원 프롬프트 추가.
이유신 - 침대를 집 내부에 자동생성되도록 배치.
김보근 - Key rebinding 기능 추가.

### ✨시연 영상 링크

---
