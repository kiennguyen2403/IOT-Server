#include <SimpleDHT.h>
#define sensorPin A0
#define soundPin 13
#define tempPin 11
#define livingroom 4
#define bedroom 2
SimpleDHT11 dht11(tempPin);

unsigned long currentTime;
int sound = 1;
int temp;
bool livingroomState = false;
bool bedroomState = false;

void ledcontroller()
{
  if (sound == 0)
  {
    digitalWrite(livingroom, !livingroomState);
    digitalWrite(bedroom, !bedroomState);
  }
}


void sendData()
{
  Serial.write(temp);
  Serial.write(sound);
}

void readData()
{
  temp = digitalRead(tempPin);
  sound = digitalRead(soundPin);
  Serial.println(temp);
  Serial.println(sound);
}



void setup()
{
  Serial.begin(9600);
  pinMode(tempPin, INPUT);
  pinMode(soundPin, INPUT);
  pinMode(livingroom, OUTPUT);
  pinMode(bedroom, OUTPUT);

  // put your setup code here, to run once:
}

void loop()
{
  // put your main code here, to run repeatedly:
  static unsigned long startTime = 0;

  currentTime = millis();

  if (startTime == 0)
  {
    sendData();
    startTime = currentTime;
  }
  if (currentTime - startTime >= 3600000UL)
  {
    // reste start time; next iteration of loop a temperature reading will be done because startTime equals 0
    startTime = 0;
  }

  readData();
  // if (flag == true) {
  //     play();
  //   }
}