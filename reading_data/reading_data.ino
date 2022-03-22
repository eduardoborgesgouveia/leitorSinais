#include "Timer.h"

Timer t;

#define pot A0


const byte ok = 0x01;
const byte erro = 0x02;
const byte acender = 0x10;
const byte apagar = 0x11;
const byte ler = 0x12;
//const byte dado;

const byte ST = 0x53;
const byte ET = 0x04;
const byte PL = 2;



void setup()
{
  Serial.begin(57600);    //configura comunicação serial com 9600 bps
  analogReadResolution(12);
  t.every(10, takeReading);      
}


void loop() 
{
  t.update();
   
}
void takeReading()
{
  uint16_t dado = analogRead(pot);
  //Padrão MSB First: O primeiro byte sempre corresponde ao byte mais significativo
  
  Serial.write(ST); //Start 
  Serial.write(PL); //Quantidade de informação
  Serial.write(dado>>8); //MSB
  Serial.write(dado&0xFF); //LSB
  Serial.write(ET); //End   
  
  
  /*Serial.println(ST);
  Serial.println(PL);
  Serial.println(dado>>8);
  Serial.println(dado&0xFF);
  Serial.println(ET);
  */
  
  
  //Serial.println(String(dado) + " " + String(dado>>8) + " " + String(dado&0xFF));
  
}

