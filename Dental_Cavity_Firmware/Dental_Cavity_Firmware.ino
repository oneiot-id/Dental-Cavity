
unsigned long timer, timeoutTimer;
bool startTransmit = false;


void handleTransmission() {
  if (timer - timeoutTimer > 1000) {
    startTransmit = false;
  } 
  else {
    startTransmit = true;
  }

  //for now we always send it to check
  if(startTransmit)
    send();

  if (Serial.available() > 0) {
    char data = Serial.read();

    if (data == '*') {
      Serial.print("connect");
      // Serial.println("Reconnecting");
      timeoutTimer = timer;
    }
  }
}

void send()
{
  //We simulate this with an potensiometer, if the PCB arrive this will be other sensor
  //bruh too many c#
  int potVal = analogRead(A0);

  uint8_t potLow = potVal & 0xFF;
  uint8_t potHigh = potVal >> 8;

  uint8_t data[] = {'*', potLow, potHigh, '#'};

  for(auto d : data){
    Serial.write(d);
  }
}

void setup() {
  Serial.begin(115200);

  Serial.println("Starting connection...");

  while (!startTransmit) {
    if (Serial.available()) {
      char data = Serial.read();

      if (data == '*') {
        Serial.print("connect");
        startTransmit = true;
      }
    }

    timer = timeoutTimer = millis();
  }

  pinMode(A0, INPUT);
  Serial.println("Connected");
}

void loop() {
  timer = millis(); 

  handleTransmission();

  // Serial.println(analogRead(A0));
}
