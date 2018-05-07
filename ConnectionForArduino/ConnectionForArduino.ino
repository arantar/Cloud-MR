#include <ArduinoJson.h>
#include <Wire.h>
#include <TroykaIMU.h>
#include <TinyGPS++.h>
#include <LiquidCrystal_I2C.h>

const uint32_t GPSBaud = 115200;
const uint32_t IMUBaud = 115200;
const uint32_t ESP8266Baund = 115200;

unsigned int idJsonPackage = 0;
unsigned long nowMillis = 0;
unsigned int analogValue = 0;
float voltage = 0;

Gyroscope gyro;
Accelerometer accel;
Compass compass;
Barometer barometer;
TinyGPSPlus gps;
LiquidCrystal_I2C lcd(0x27, 2, 1, 0, 4, 5, 6, 7, 3, POSITIVE);

void setup() {
	lcd.begin(16, 2);
	Serial.begin(9600);
	Serial1.begin(ESP8266Baund);
	Serial2.begin(GPSBaud);
	gyro.begin();
	accel.begin();
	compass.begin();
	barometer.begin();
	pinMode(A0, INPUT);
}

void loop() {
	GenerationJson();
	if (millis() - nowMillis >= 5000) {
		nowMillis = millis();
		ReadingBatteryVoltage();
		DisplayOutput();
	}
}

void DisplayOutput() {
	lcd.backlight();
	lcd.setCursor(0, 0);
	lcd.print("Voltage: ");
	lcd.setCursor(9, 0);
	lcd.print(voltage);
	lcd.setCursor(0, 1);
	lcd.print("Sent: ");
	lcd.setCursor(6, 1);
	lcd.print(idJsonPackage);
	lcd.setCursor(12, 1);
	lcd.print(millis() / 1000);
}

void ReadingBatteryVoltage() {
	analogValue = analogRead(A0);
	voltage = ((float)analogValue * 5) / 1023;
}

void GenerationJson() {
	StaticJsonBuffer<300> jsonBuffer;
	JsonObject& dataSensors = jsonBuffer.createObject();
	String str = "";

	dataSensors["id"] = idJsonPackage;
	dataSensors["time"] = millis() / 1000;

	JsonObject& IMU = dataSensors.createNestedObject("IMU");
	JsonArray& IMU_gyroscope = IMU.createNestedArray("gyroscope");
	IMU_gyroscope.add(gyro.readDegPerSecX());
	IMU_gyroscope.add(gyro.readDegPerSecY());
	IMU_gyroscope.add(gyro.readDegPerSecZ());

	JsonArray& IMU_accelerometer = IMU.createNestedArray("accelerometer");
	IMU_accelerometer.add(accel.readAX());
	IMU_accelerometer.add(accel.readAY());
	IMU_accelerometer.add(accel.readAZ());

	//JsonArray& Magnetometer = IMU.createNestedArray("magnetometer");
	//Magnetometer.add(compass.readAzimut());
	//Magnetometer.add(compass.readGaussX());
	//Magnetometer.add(compass.readGaussY());
	//Magnetometer.add(compass.readGaussZ());

	IMU["pressure"] = barometer.readPressureMillibars();
	IMU["temperature"] = barometer.readTemperatureC();

	JsonObject& GPS = dataSensors.createNestedObject("GPS");
	GPS["gpsSat"] = gps.satellites.value();
	GPS["gpsHDOP"] = gps.hdop.value();
	GPS["gpsLat"] = gps.location.lat();
	GPS["gpsLon"] = gps.location.lng();
	GPS["gpsDate"] = gps.date.value();
	GPS["gpsTime"] = gps.time.value();
	GPS["gpsAlt"] = gps.altitude.meters();
	GPS["gpsSpeed"] = gps.speed.kmph();

	dataSensors.prettyPrintTo(str);
	str = "<" + str + ">";
	Serial1.println(str);
	idJsonPackage++;
	delay(50);
}

