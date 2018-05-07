//#include <SoftwareSerial.h>
#include <WiFiUdp.h>
#include <ESP8266WiFi.h>
#include <ESP8266WiFiMulti.h>

//SoftwareSerial ESPserial(13, 15);

const char *ssid = "Notebook";
const char *password = "12345670";

const uint16_t port = 11000;
const char *host = "192.168.137.1";
const int sizeBuffer = 500;

WiFiUDP Udp;
ESP8266WiFiMulti WiFiMulti;

void setup() {
	Serial.begin(115200);
	Serial.swap();
	//ESPserial.begin(38400);
	WiFiMulti.addAP(ssid, password);
	while (WiFiMulti.run() != WL_CONNECTED) {
		delay(500);
	}
	delay(500);
}

void loop() {
	char incomingChars[sizeBuffer] = "";
	for (int i = 0; i < sizeBuffer; i++) {
		if (Serial.available()) {
			incomingChars[i] = Serial.read();
			if (incomingChars[i] == '>') {
				break;
			}
		}
		else {
			i--;
		}
	}
	Udp.beginPacket(host, 11000);
	Udp.write(incomingChars);
	Udp.endPacket();
}
