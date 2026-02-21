using System;
Random rand = new Random();

// === 플레이어 스탯 ===
float playerHP = 1000;  // 플레이어 HP
int playerMP = 300; // 플레이어 MP

const int playerAttackDamage = 200; // 플레이어 공격력
const int playerSkillDamage = 500; // 플레이어 스킬 공격력 (스킬을 사용하려면 MP 100 소모)
const int playerDefense = 50;   // 플레이어 방어력

const int criticalProbality = 30; // 크리티컬 확률 (크리티컬이면 공격력 1.5배)

bool isStunned = false; // 기절 상태 플래그
bool isDefencing = false;   // 방어 상태 플래그

int playerActionNumber = 0;   // 플레이어 행동선택
float nextDamage = 0;

// === 검은 마법사 스탯 ===
float enemyHP = 5000;   // 검마 HP

const int enemyAttack = 300;    // 검마 공격력
const int enemyDefense = 100;   // 검마 방어력

bool isCursing = (enemyHP <= 2500); // HP 2500 이하일 때 발동 (플레이어 HP 50 감소, 1턴 기절 효과)
                                    // 검마 HP 변경 있을 때 재할당 필요함.

int curseCount = 0; // 검마의 저주 스킬 쿨타임. (사용 시 3으로 초기화, 0이면 저주가 발동하고, 쿨타임은 3턴.)

Console.WriteLine($"\n  ### 전투 시작! ###\n");

while (true)    // 승부가 날 때까지 무한 반복합니다.
{
    Console.WriteLine($"  >>> 플레이어의 턴 <<<\n");
   
    {
        // 1. 저주 패턴이 발동하는지 확인
        // 1-1. 저주가 발생한 경우 : 플레이어는 HP가 50 감소하고, 기절 상태를 획득합니다.
        // 1-2. 다음 저주 발동까지 쿨타임이 남은 경우 : 위의 효과는 발생하지 않고, 쿨타임 카운트가 1 감소합니다.

        if (isCursing)      // 1. 저주 패턴이 발동하는지 확인
        {
            if (curseCount == 0)    // 1-1. 저주가 발생한 경우 : 플레이어는 HP가 50 감소하고, 기절 상태를 획득합니다.
            {
                Console.WriteLine("  - 저주 발생! \n");
                Console.WriteLine("  - 플레이어 HP -50 / 기절 상태 획득\n");
                Console.WriteLine($"  [ 플레이어 HP : {playerHP} ]\n");

                playerHP -= 50; isStunned = true;
                curseCount = 3;
            }

            else    // 1-2. 다음 저주 발동까지 쿨타임이 남은 경우 : 위의 효과는 발생하지 않고, 쿨타임 카운트가 1 감소합니다.
            {
                playerHP -= 50;    // 윤지수 작성 : '저주 패턴 발동 시 매 턴 플레이어 HP 50 감소' 규칙이 있었습니다.
                curseCount--;
            }

            Console.WriteLine($"  - 다음 저주까지 {curseCount} 라운드 \n");
            //if (isStunned)    // 윤지수 작성 : '저주 패턴으로 스턴 상태일 때 해당 턴 건너뜀' 조건이 있었습니다.
            //{
            //   continue;
            //}
        }
    }

    // 윤지수님 파트 ▼    // 쓰다보니 양이 너무 많아서 윗부분을 뺐어요!
    {
        // 1. 플레이어의 행동
        // 1-1. 플레이어가 기절 상태라면, 아무것도 하지 않고 넘어갑니다. 기절 상태를 해제시킵니다.
        // 1-2. 플레이어가 기절하지 않았다면, (1.공격/2.스킬/3.방어) 중 유효한 입력이 들어올 때까지 반복하여 입력을 받습니다.
        if (isStunned)
        {
            isStunned = false;
        }
        else
        {
            Console.WriteLine("  - 플레이어 행동 선택 (1~3 숫자 입력)");
            Console.Write("  [ 1. 일반 공격    2. 스킬 사용    3. 방어 ] : ");
            while(true) 
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out playerActionNumber) && (playerActionNumber == 1 || playerActionNumber==2 || playerActionNumber == 3))
                {
                    if (playerActionNumber == 2 && playerMP < 100)
                    {
                        Console.WriteLine("  - MP 부족으로 스킬 사용 불가\n");
                        Console.Write("  - 다시 입력 : ");
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                else if (!int.TryParse(input, out playerActionNumber) || playerActionNumber < 1 || playerActionNumber > 3)
                {
                Console.WriteLine("  - 1~3 사이의 숫자만 입력 가능\n");
                Console.Write("  - 다시 입력 : ");
                
                }
            }
        }
        switch (playerActionNumber)
        {
            // 1-1-1. 일반 공격, 검은마법 체력 감소 후 잔여 체력 출력했습니다.
            case 1:
                enemyHP = enemyHP - (enemyDefense + playerAttackDamage);  
                Console.WriteLine($"  - {playerActionNumber}. 일반 공격 선택\n");
                Console.WriteLine($"  - 검은 마법사 HP {enemyDefense-playerAttackDamage}\n");
                Console.WriteLine($"  [ 검은 마법사 HP : {enemyHP} ]\n");    // 
                break;
            // 1-1-2. 스킬 (*MP가 100 미만이라면 사용할 수 없습니다.)
            case 2:
                enemyHP = enemyHP - (enemyDefense - playerSkillDamage);
                Console.WriteLine($"  - {playerActionNumber}. 스킬 사용 선택\n");
                Console.WriteLine($"  - 검은 마법사 HP {enemyDefense - playerSkillDamage}\n");
                Console.WriteLine($"  [ 검은 마법사 HP : {enemyHP} ]\n");    // 
                playerMP -= 100;
                break ;
            // 1-1-3. 방어
            case 3:
                isDefencing = true;
                Console.WriteLine($"  - {playerActionNumber}. 방어 선택\n");
                break;
        }
        // 각각 올바른 작업을 하도록 작성해 주세요.
    }
    // 윤지수님 파트 ▲

    {
        // 1. 크리티컬 발동 여부를 판단합니다. (저는 Random 함수를 사용했어요.)
            // 1-2. 크리티컬이 발동했다면 플레이어의 공격 데미지를 1.5배로 만듭니다.
        if (!isDefencing)
        {
            if (rand.Next(0, 100) <= criticalProbality)
            {
                Console.WriteLine("  ▶ 크리티컬! ◀\n");
                nextDamage *= 1.5f;
            }

            // 2. 플레이어의 공격 데미지와 적의 방어력을 고려해 적의 HP를 감소시킵니다.
            enemyHP -= (nextDamage - enemyDefense);
        }

        Console.WriteLine($"  [ 검은 마법사 HP: {enemyHP} ]\n");
        isCursing = (enemyHP <= 2500);

        // 3. 만약 검마의 체력이 0 이하가 되었다면 플레이어가 승리했음을 알리고, 반복문을 빠져나갑니다.
        if (enemyHP <= 0)
        {
            Console.WriteLine("  ========== 플레이어의 승리! ==========\n");
            break;
        }
    }

    Console.WriteLine($"  >>> 검은 마법사의 턴 <<<\n");

    // 이현석님 파트 ▼
    {
        // 1-1. 만약 검마의 HP이 1000 이하라면 특수 패턴을 사용해요. (데미지가 2배)
        // 1-2. 아니라면 기본 데미지로 공격합니다.

        // 2. 플레이어가 방어 중이라면... (데미지 경감 또는 무효화? 효과는 자유롭게 정해도 돼요.)

        // 3. 검마의 공격 데미지와 플레이어의 방어력을 고려해 플레이어의 HP를 감소시킵니다.

        // 4. 만약 플레이어의 체력이 0 이하가 되었다면 플레이어가 패배했음을 알리고, 반복문을 빠져나갑니다.
    }
    // 이현석님 파트 ▲
}