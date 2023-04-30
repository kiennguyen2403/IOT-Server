#define ultraSonicLivingTriggerPin 11
#define ultraSonicLivingEchoPin 10

#define ultraSonicBedroomTriggerPin 6
#define ultraSonicBedroomEchoPin 5

#define flamePin A4


#define soundPin 13
#define soundA0Pin A0

#define tempPin 9

#define livingroom 4
#define bedroom 2
#define fan 3
#define buzzer 8


// global value 
unsigned long currentTime;
int sound = 1;

byte temperature = 0;


bool livingroomState = LOW;
bool bedroomState = LOW;
bool buzzerState = LOW;
bool isLivingRoomHavePeople = false;
bool isbedroomHavePeople = false;
bool isStageChange = false;
bool isTimer = false;
bool isFire = false;
int timer = 1;

String command = "";
unsigned long startTime;


void detectFire(){
  int flame = analogRead(flamePin);
  if (flame > 1022)
  {
    isFire = true;
    buzzerState = HIGH;
    isStageChange = true;  
  } else {
    isFire = false;
    buzzerState = LOW;
    isStageChange = true;  
  }
}

void timermode(){
  unsigned long currentTime = millis();                
  unsigned long elapsedTime = currentTime - startTime; 
  if (elapsedTime >= timer * 60 * 60 * 1000)
  {                      
    livingroomState = LOW;
    bedroomState = LOW;
    startTime = millis();
    isStageChange = true;
  }
 }

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
    isStageChange = true;
  }

  if (command == "2on")
  {
    livingroomState = HIGH;
  }
  else if (command == "2off")
  {
    livingroomState = LOW;
  }
  else if (command == "1on")
  {
    bedroomState = HIGH;
  }
  else if (command == "1off")
  {
    bedroomState = LOW;
  }
  else if (command == "warning1")
  {
    buzzerState = HIGH;
  } 
  else if (command == "warning0")
  {
    buzzerState = LOW;
  } else if(command == "disabletimer"){
    isTimer = false;
  }
  else if (command.indexOf("timer") != -1 && command != "disabletimer")
  {
    isTimer = true;
    startTime = millis();
    timer = command.substring(5).toInt();
  }

  if (isTimer)
  {
    timermode();
  }
  digitalWrite(buzzer, buzzerState);
  digitalWrite(bedroom, bedroomState);
  digitalWrite(livingroom, livingroomState);
  command = "";
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

    if (isFire)
    {
      Serial.write("fire1");
    }
    else{
      Serial.write("fire0");
    }

    isStageChange = false;
  }
}

void readCommand(){
  if (Serial.available())
  {
    command = Serial.readString();
  }
}



void readData()
{
  detectFire();
  isLivingRoomHavePeople = detecthuman("livingroom");
  isbedroomHavePeople = detecthuman("bedroom");
  sound = digitalRead(soundPin);
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

  pinMode(flamePin, INPUT);

  pinMode(livingroom, OUTPUT);
  pinMode(bedroom, OUTPUT);
  pinMode(buzzer, OUTPUT);

  startTime = millis();
  Serial.setTimeout(250);
}


void loop()
{
  readData();
  devicecontroller();
  sendData();
}