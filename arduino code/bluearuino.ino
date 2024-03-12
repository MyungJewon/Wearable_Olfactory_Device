#include<SoftwareSerial.h>
int blueTx=2;   //Tx (보내는핀 설정)at
int blueRx=3;   //Rx (받는핀 설정)
SoftwareSerial BTSerial(blueTx,blueRx);     //블루투스 pin설정

int fan1 = 4;     //각팬을 제어할 digital pin number
int fan2 = 5;
int fan3 = 6;
int fan4 = 7;
int fan5 = 8;

void setup() {
  Serial.begin(115200);   //유선연결 비트레이트
  BTSerial.begin(115200); //블루투스연결 비트레이트

  pinMode(fan1, OUTPUT);  //각팬의 작동을 정지 Setup
  digitalWrite(fan1, LOW);

  pinMode(fan2, OUTPUT);
  digitalWrite(fan2, LOW);

  pinMode(fan3, OUTPUT);
  digitalWrite(fan3, LOW);

  pinMode(fan4, OUTPUT);
  digitalWrite(fan4, LOW);

  pinMode(fan5, OUTPUT);
  digitalWrite(fan5, LOW);

  BTSerial.setTimeout(10); //데이터 를 즉시 방영하지 않고 10ms의 대기시간을 가짐 (없으면 오작동) 
}

void loop() {
  if (BTSerial.available())         //블루투스에서 데이터가 올시 내부로 출력
    Serial.write(BTSerial.read());
  if (Serial.available())           //내부에서 데이터가 올시 블루투스로 출력
    BTSerial.write(Serial.read());


  char fanvalue = BTSerial.parseInt();  //블루투스에서 전송받은 데이터를 int값으로 변경 (유니티에서는 char로 전송됨)
  //fanvalue= Serial.parseInt();
  switch(fanvalue){               //fanvalue에 따라 작동하는 팬 제어
    case 3:
      Serial.print("off");
      digitalWrite(fan1,LOW);
      digitalWrite(fan2,LOW);
      digitalWrite(fan3,LOW);
      digitalWrite(fan4,LOW);
      digitalWrite(fan5,LOW);
      break;
    case 4:
      Serial.print("1Fan off");
      digitalWrite(fan1, LOW);
      break;
    case 5:
      Serial.print("1Fan on");
      digitalWrite(fan1,HIGH);
      digitalWrite(fan2,LOW);
      digitalWrite(fan3,LOW);
      digitalWrite(fan4,LOW);
      digitalWrite(fan5,LOW);
      break;
    case 6:
      Serial.print("2Fan off");
      digitalWrite(fan2,LOW);
      break;
    case 7:
      Serial.print("2Fan on");
      digitalWrite(fan2,HIGH);
      digitalWrite(fan1,LOW);
      digitalWrite(fan3,LOW);
      digitalWrite(fan4,LOW);
      digitalWrite(fan5,LOW);
      break;
    case 8:
      Serial.print("3Fan off");
      digitalWrite(fan3,LOW);
      break;
    case 9:
      Serial.print("3Fan on");
      digitalWrite(fan3,HIGH);
      digitalWrite(fan1,LOW);
      digitalWrite(fan2,LOW);
      digitalWrite(fan4,LOW);
      digitalWrite(fan5,LOW);
      break;
    case 10:
      Serial.print("4Fan off");
      digitalWrite(fan4,LOW);
      break;
    case 11:
      Serial.print("4Fan on");
      digitalWrite(fan4,HIGH);
      digitalWrite(fan1,LOW);
      digitalWrite(fan2,LOW);
      digitalWrite(fan3,LOW);
      digitalWrite(fan5,LOW);
      break;
    case 12:
      Serial.print("5Fan off");
      digitalWrite(fan5,LOW);
      break;
    case 13:
      Serial.print("5Fan on");
      digitalWrite(fan5,HIGH);
      digitalWrite(fan1,LOW);
      digitalWrite(fan2,LOW);
      digitalWrite(fan3,LOW);
      digitalWrite(fan4,LOW);
      break;
    case 14:
      Serial.print("5,3 Fan on");
      digitalWrite(fan5,HIGH);
      digitalWrite(fan1,LOW);
      digitalWrite(fan2,LOW);
      digitalWrite(fan3,HIGH);
      digitalWrite(fan4,LOW);
      break;
      //이 코드는 변경시 유니티에서 전송하는 부분의 데이터도 변경해야함
  }
}
