#include <SimpleDHT.h>
#include "buzzer.h"

//define const
#define ultraSonicLivingTriggerPin 11
#define ultraSonicLivingEchoPin 10

#define ultraSonicBedroomTriggerPin 6
#define ultraSonicBedroomEchoPin 5


#define soundPin 13
#define soundA0Pin A0

#define tempPin 11

SimpleDHT11 dht11(tempPin);

#define livingroom 4
#define bedroom 2
#define fan 3
#define buzzer 1


// global value 
unsigned long currentTime;
int sound = 1;

byte temperature = 0;


bool livingroomState = LOW;
bool bedroomState = LOW;
bool isLivingRoomHavePeople = false;
bool isbedroomHavePeople = false;
bool isStageChange = false;
String command = "";


bool detecthuman(String room){
  if (room == "livingroom")
  {
      int duration, distance;
      digitalWrite(ultraSonicLivingTriggerPin, LOW);
      delayMicroseconds(2);
      digitalWrite(ultraSonicLivingTriggerPin, HIGH);
      delayMicroseconds(10);
      digitalWrite(ultraSonicLivingTriggerPin, LOW);
      duration = pulseIn(ultraSonicLivingEchoPin, HIGH);
      distance = duration * 0.034 / 2;
      if (distance < 20)
      {
        return true;
      }
      else
      {
        return false;
      }
  } else {
      int duration, distance;
      digitalWrite(ultraSonicBedroomTriggerPin, LOW);
      delayMicroseconds(2);
      digitalWrite(ultraSonicBedroomTriggerPin, HIGH);
      delayMicroseconds(10);
      digitalWrite(ultraSonicBedroomTriggerPin, LOW);
      duration = pulseIn(ultraSonicBedroomEchoPin, HIGH);
      distance = duration * 0.034 / 2;
      if (distance < 20)
      {
        return true;
      }
      else
      {
        return false;
      }
  }
}


void devicecontroller()
{
  if (sound == 0 && isbedroomHavePeople)
  {
    if (bedroomState)
    {
      bedroomState = LOW;
    }
    else
    {
      bedroomState = HIGH;
    }
    digitalWrite(bedroom, bedroomState);
    isStageChange = true;
  } else if (sound == 0 && isLivingRoomHavePeople) {
    if (livingroomState)
    {
      livingroomState = LOW;
    }
    else
    {
      livingroomState = HIGH;
    }
    digitalWrite(livingroom, livingroomState);
    isStageChange = true;
  }

  if (command == "2on")
  {
    livingroomState = HIGH;
    digitalWrite(livingroom, livingroomState);
    isStageChange = true;
  }
  else if (command == "2off")
  {
    livingroomState = LOW;
    digitalWrite(livingroom, livingroomState);
    isStageChange = true;
  }
  else if (command == "1on")
  {
    bedroomState = HIGH;
    digitalWrite(bedroom, bedroomState);
    isStageChange = true;
  }
  else if (command == "1off")
  {
    bedroomState = LOW;
    digitalWrite(bedroom, bedroomState);
    isStageChange = true;
  }
  else if (command == "warning1")
  {
    digitalWrite(1, HIGH);
  } else if (command == "fan1")
  {
    digitalWrite(fan, HIGH);
  } else if (command == "fan0")
  {
    digitalWrite(fan, LOW);
  } else if (command == "warning0")
  {
    digitalWrite(1, LOW);
  }

  command = ""; // reset command

  if ((int)temperature > 30)
  {
    digitalWrite(fan, HIGH);
  }
  else
  {
    digitalWrite(fan, LOW);
  }
}


void sendData()
{
  if (isStageChange)
  {
    if (livingroomState)
    {
      Serial.write("livingroom1");
    }
    else{
      Serial.write("livingroom0");
    }

    if (bedroomState)
    {
      Serial.write("bedroom1");
    }
    else{
      Serial.write("bedroom0");
    }

    isStageChange = false;
  }
}
void readTemp()
{

  byte humidity = 0;
  int err = SimpleDHTErrSuccess;
  if ((err = dht11.read(&temperature, &humidity, NULL)) != SimpleDHTErrSuccess)
  {
    Serial.print("Read DHT11 failed, err="); Serial.println(err);
    return;
  }
  Serial.print("Sample OK: ");
  Serial.print((int)temperature); Serial.print(" *C, ");
  Serial.print((int)humidity); Serial.println(" H");
}


void readCommand(){
  if (Serial.available())
  {
    command = Serial.readString();
  }
}



void readData()
{
  isLivingRoomHavePeople = detecthuman("livingroom");
  isbedroomHavePeople = detecthuman("bedroom");
  // temp = digitalRead(tempPin);
  sound = digitalRead(soundPin);
  // readTemp();
  readCommand();
}


void setup()
{
  // put your setup code here, to run once:
  Serial.begin(9600);
  pinMode(tempPin, INPUT);

  pinMode(soundPin, INPUT);
  pinMode(soundA0Pin, INPUT);

  pinMode(ultraSonicLivingTriggerPin, OUTPUT);
  pinMode(ultraSonicLivingEchoPin, INPUT);

  pinMode(ultraSonicBedroomTriggerPin, OUTPUT);
  pinMode(ultraSonicBedroomEchoPin, INPUT);

  pinMode(livingroom, OUTPUT);
  pinMode(bedroom, OUTPUT);
  pinMode(fan, OUTPUT);

  digitalWrite(buzzer, LOW);

  Serial.setTimeout(250);
}


void loop()
{
  readData();
  devicecontroller();
  sendData();
}