#include <ArduinoMqttClient.h>
#include <ESP8266WiFi.h>

#define SSID_1 "[HCI LAB : SKYNET 2]"
#define PSK_1  "hcifreewifisabyjung"

#define SSID_2 "[HCI LAB]"
#define PSK_2  "hcifreewifisabyjung"

#define BROKER_ADDR  "192.168.2.100"
#define BROKER_PORT 1883

#define TOPIC_SUB_LIGHT "/mr/object/lamp"

#define PIN_IO_LIGHT  D1

WiFiClient wifiClient;
MqttClient mqttClient(wifiClient);

// the setup function runs once when you press reset or power the board
void setup()
{

#pragma region Serial Port

  Serial.begin(115200);
  while (!Serial)
    ;
  Serial.println();
  Serial.println("Startup...");
  Serial.println("Initialize I/O...");

#pragma endregion

#pragma region I/O

  pinMode(PIN_IO_LIGHT, OUTPUT);
  
#pragma endregion

#pragma region WiFI

  bool isConnected = false;

  do
  {

    isConnected = false;

    // # MOBILE 1
    Serial.print("Connecting to ");
    Serial.print(SSID_1);

    WiFi.hostname("Smart Plug");
    WiFi.mode(WIFI_STA);
    WiFi.begin(SSID_1, PSK_1);
    for (int t = 0; t < 10; t++)
    {

      if (WiFi.status() != WL_CONNECTED)
      {
        Serial.print(".");

        digitalWrite(PIN_IO_LIGHT, HIGH);
        delay(500);
        digitalWrite(PIN_IO_LIGHT, LOW);
        delay(500);
        
      }
      else
      {
        isConnected = true;
        break;
      }

    }
    Serial.println();

    if (isConnected == false)
    {

      // # HCI LAB
      Serial.print("Connecting to ");
      Serial.print(SSID_2);

      WiFi.mode(WIFI_STA);
      WiFi.begin(SSID_2, PSK_2);
      for (int t = 0; t < 10; t++)
      {

        if (WiFi.status() != WL_CONNECTED)
        {
          delay(500);
          Serial.print(".");

          digitalWrite(PIN_IO_LIGHT, HIGH);
          delay(500);
          digitalWrite(PIN_IO_LIGHT, LOW);
          delay(500);

        }
        else
        {
          isConnected = true;
          break;
        }

      }
      Serial.println();

    }

  } while ((WiFi.status() != WL_CONNECTED) || isConnected == false);

  Serial.println("");
  Serial.println("WiFi connected");
  Serial.print("IP address ");
  Serial.println(WiFi.localIP());

#pragma endregion

#pragma region MQTT

  Serial.print("Connecting to MQTT Broker ");
  Serial.print(BROKER_ADDR);
  Serial.print(":");
  Serial.println(BROKER_PORT);

  mqttClient.setId("Smart Plug - 2020-01-19 A");
  if (!mqttClient.connect(BROKER_ADDR, BROKER_PORT))
  {

    Serial.print("MQTT connection failed! Error code = ");
    Serial.println(mqttClient.connectError());
    Serial.println("Please Restart");

    while (1)
    {

      digitalWrite(PIN_IO_LIGHT, HIGH);
      delay(500);
      digitalWrite(PIN_IO_LIGHT, LOW);
      delay(500);

    }

  }

  Serial.println("MQTT Broker connected");
  mqttClient.onMessage(MyOnMqttMessage);

  Serial.println("Subscribing ...");
  mqttClient.subscribe(TOPIC_SUB_LIGHT);

  Serial.println("Ready");

  digitalWrite(PIN_IO_LIGHT, HIGH);

#pragma endregion


}

// the loop function runs over and over again until power down or reset
void loop()
{

  mqttClient.poll();

}

void MyOnMqttMessage(int messageSize) {
  // we received a message, print out the topic and contents
  Serial.print("Received a message with topic '");
  Serial.print(mqttClient.messageTopic());
  Serial.print("', length ");
  Serial.print(messageSize);
  Serial.println(" bytes:");

  // use the Stream interface to print the contents
  char data;
  while (mqttClient.available())
  {
    data = mqttClient.read();
    Serial.print(data);
  }
  Serial.println();
  Serial.println();

  if (data == '0')  // Turn the Light off
  {
    Serial.println("Turn the light : Off");
    digitalWrite(PIN_IO_LIGHT, HIGH);
  }
  else if (data == '1') // Turn the Light on
  {
    Serial.println("Turn the light : On");
    digitalWrite(PIN_IO_LIGHT, LOW);
  }

}
