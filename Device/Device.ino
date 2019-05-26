#include <SoftwareSerial.h>//include library code

#define DEBUG true

SoftwareSerial mySerial(10,11);//RX, TX
String server =  "AT+HTTPPARA=\"URL\",\"http://664180d4-81ce-423e-bc8f-0d0630eea365.mock.pstmn.io/Log?Id=1&data=";
//String sendd="AT+HTTPPARA=\"URL\",\"http://664180d4-81ce-423e-bc8f-0d0630eea365.mock.pstmn.io/Log\?Id=1&data=1111111\"";

void setup(){
  Serial.begin(9600);
  mySerial.begin(9600); 
  SetUpDevice();
  ActivateGprsCaperbility();
}

void loop(){
  String datat = SendCommand( "AT+CGNSINF",1000,DEBUG);
         datat = Format(datat); 
         SendLocationToServer(datat);
         delay(10000);     
         delay(10000);     
         delay(10000);     
}


//device start up
void SetUpDevice(void){ 
  //sim ready  
    SendCommand( "AT+CPIN?",1000,DEBUG);   
  //error msg activate 
    SendCommand( "AT+CMEE=2",1000,DEBUG); 
//Connet Gprs network
//    SendCommand( "At+cgatt=1",1000,DEBUG);  
  //Add APN
//    SendCommand( "AT+CSTT=\"mobitel\",\"\",\"\"",1000,DEBUG);  
  //Activate Wirless Caperbiliities
//    SendCommand( "AT+CIICR",1000,DEBUG);  
  //  Power up Gps
    SendCommand( "AT+CGNSPWR=1",1000,DEBUG);  
}


String SendCommand(String command, const int timeout, boolean debug){
    String response = "";  
    bool flag =false;
    while(!flag)
    {  
      response = "";
      mySerial.println(command); 
      delay(5);
      if(debug)
      {
        long int time = millis();   
        while( (time+timeout) > millis())
        {
            while(mySerial.available())
            {       
              response += char(mySerial.read());
            }  
        }   
//        Serial.println(response);    
        // check for OK in responce if not loop        
        byte index = response.indexOf("OK");                     
        if(index!=-1&&index!=255)
        {
//          Serial.print("count :");
          Serial.println(index);
          flag=true;
//          Serial.println("++++++++++++++++++++++++++++");
          return response;
        }
        //print error responce
      Serial.println(response);
      delay(5000);        
      } 
      
    }     
}

String Format(String response)
{
  response.remove(0,23);  
  int ind =response.lastIndexOf("OK");
  ind-=4;
  response.remove(ind,8);       
  return response;
}
void ActivateGprsCaperbility()
{
  SendCommand( "AT+SAPBR=3,1,\"CONTYPE\",\"GPRS\"",1000,DEBUG);
  SendCommand( "AT+SAPBR=3,1,\"APN\",\"mobitel\"",1000,DEBUG);
  SendCommand( "AT+SAPBR=1,1",1000,DEBUG);
  SendCommand( "AT+HTTPINIT",1000,DEBUG);
  SendCommand( "AT+HTTPPARA=\"CID\",1 ",1000,DEBUG);
}
void SendLocationToServer(String locationstring)
{
//  delay(1000);
  String datapacket =server;
  bool re =datapacket.concat(locationstring);
  datapacket.concat("\"");
  SendCommand2(datapacket);
  delay(1000);
  HttpAction();
}

void SendCommand2(String command){ 
    String response = "";
    mySerial.println(command); 
    delay(5);
    while(mySerial.available())
    {      
      response += char(mySerial.read());
    } 
//    Serial.println("************************************");
    Serial.println(response);    
} 
void HttpAction(){
    String response = "";  
    mySerial.println("AT+HTTPACTION=0"); 
    delay(2000);
    while(mySerial.available())
    {       
      response += char(mySerial.read());
    }  
//    Serial.println(response);     
}  
