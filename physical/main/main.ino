#include <SimpleDHT.h>
#include "buzzer.h"

//define const
#define ultraSonicLivingTriggerPin 11
#define ultraSonicLivingEchoPin 10

#define ultraSonicBedroomTriggerPin 6
#define ultraSonicBedroomEchoPin 5


#define soundPin 13

#define tempPin 11
#define tempA0Pin A0
SimpleDHT11 dht11(tempPin);

#define livingroom 4
#define bedroom 2


// global value 
unsigned long currentTime;
int sound = 1;
int temp = 0;
bool livingroomState = LOW;
bool bedroomState = LOW;
bool isLivingRoomHavePeople = false;
bool isbedroomHavePeople = false;
bool isStageChange = false;

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


void ledcontroller()
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

void readData()
{
  
  isLivingRoomHavePeople = detecthuman("livingroom");
  isbedroomHavePeople = detecthuman("bedroom");
  // temp = digitalRead(tempPin);
  sound = digitalRead(soundPin);
  Serial.println(sound);
}


void setup()
{
  // put your setup code here, to run once:
  Serial.begin(9600);
  pinMode(tempPin, INPUT);
  pinMode(soundPin, INPUT);


  pinMode(livingroom, OUTPUT);
  pinMode(bedroom, OUTPUT);


  pinMode(ultraSonicLivingTriggerPin, OUTPUT);
  pinMode(ultraSonicLivingEchoPin, INPUT);

  pinMode(ultraSonicBedroomTriggerPin, OUTPUT);
  pinMode(ultraSonicBedroomEchoPin, INPUT);


}


void loop()
{
  readData();
  ledcontroller();
  sendData();
}