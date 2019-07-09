DROP DATABASE IF EXISTS `semesterprojekt`;

CREATE Database if not exists `semesterprojekt`
	character set utf8
    ;
    
USE  `semesterprojekt`;

CREATE TABLE IF NOT EXISTS NTPServer (
	id INT NOT NULL AUTO_INCREMENT,
    Servername VARCHAR(45) NOT NULL,
    IPAdresse VARCHAR(45) NOT NULL,
    PRIMARY KEY(id)
    );

CREATE TABLE IF NOT EXISTS Zeitzone (
	id INT NOT NULL AUTO_INCREMENT,
    Zonenname VARCHAR(45) NOT NULL,
    Zeitabweichung INT,
    PRIMARY KEY (id)
    );
    
CREATE TABLE IF NOT EXISTS Ort (
	id INT NOT NULL AUTO_INCREMENT,
    Plz INT NOT NULL,
    Ort VARCHAR(32) NOT NULL,
    PRIMARY KEY(id)
    );

CREATE TABLE IF NOT EXISTS Kontaktperson (
	id INT NOT NULL AUTO_INCREMENT,
	Nachname VARCHAR(45) NOT NULL,
	Vorname VARCHAR(45) NOT NULL,
	PRIMARY KEY (id)
	);
    
CREATE TABLE IF NOT EXISTS Telefonnummer (
    id INT NOT NULL AUTO_INCREMENT,
    Kontaktperson_id INT NOT NULL,
    Name VARCHAR(45) NOT NULL,
    Art VARCHAR(45) NOT NULL,
    Nummer VARCHAR(45) NOT NULL,
    PRIMARY KEY (id),
    FOREIGN KEY(Kontaktperson_id)
		REFERENCES Kontaktperson(id)
		ON UPDATE CASCADE
		ON DELETE RESTRICT
	);    
    
    
CREATE TABLE IF NOT EXISTS IPInformationen (
    id INT NOT NULL AUTO_INCREMENT,
	IPAdresse VARCHAR(45) NOT NULL,
	Subnetmaske VARCHAR(45) NOT NULL,
	Gateway VARCHAR(45) NOT NULL,
	PRIMARY KEY (id)
	);

CREATE TABLE IF NOT EXISTS Creditals (
	id INT NOT NULL AUTO_INCREMENT,
    Benutzername VARCHAR(45) NOT NULL,
    Passwort VARCHAR(45) NOT NULL,
    SNMPCommunity VARCHAR(45),
    PRIMARY KEY (id)
    );
    
CREATE TABLE IF NOT EXISTS DeviceType (
	id INT NOT NULL AUTO_INCREMENT,
    DeviceType VARCHAR(45) NOT NULL,
    PRIMARY KEY (id)
    );

CREATE TABLE IF NOT EXISTS Hersteller (
	id INT NOT NULL AUTO_INCREMENT,
    Herstellername VARCHAR(45) NOT NULL,
    PRIMARY KEY (id)
    );

CREATE TABLE IF NOT EXISTS Adresse (
	id INT NOT NULL AUTO_INCREMENT,
    Strasse VARCHAR(45) NOT NULL,
    Nummer VARCHAR(5),
    Ort_id INT NOT NULL,
    PRIMARY KEY(id),
    FOREIGN KEY(Ort_id)
		REFERENCES Ort(id)
		ON UPDATE CASCADE
		ON DELETE RESTRICT
	);

CREATE TABLE IF NOT EXISTS Kunde (
	KundenNr INT NOT NULL AUTO_INCREMENT,
	Firmenname VARCHAR(45) NOT NULL,
	PRIMARY KEY(KundenNr)
	);

CREATE TABLE IF NOT EXISTS Zahlungen (
	id INT NOT NULL AUTO_INCREMENT,
	Betrag DECIMAL NOT NULL,
	Zahlungseingang DATE NOT NULL,
	PRIMARY KEY(id)
	);
    
CREATE TABLE IF NOT EXISTS Kundenkonto (
	id INT NOT NULL AUTO_INCREMENT,
	KundenNr_id INT NOT NULL,
	Zahlungen_id INT,
	PRIMARY KEY(id),
	FOREIGN KEY(KundenNr_id)
		REFERENCES Kunde(KundenNr)
		ON UPDATE CASCADE
		ON DELETE RESTRICT,
	FOREIGN KEY(Zahlungen_id)
		REFERENCES Zahlungen(id)
		ON UPDATE CASCADE
		ON DELETE RESTRICT
	);
    
CREATE TABLE IF NOT EXISTS POD (
	id INT NOT NULL AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    Kundenkonto_id INT NOT NULL,
    PRIMARY KEY(id),
	FOREIGN KEY(Kundenkonto_id)
		REFERENCES Kundenkonto(id)
		ON UPDATE CASCADE
		ON DELETE RESTRICT
    );

CREATE TABLE IF NOT EXISTS Location (
	id INT NOT NULL AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    POD_id INT NOT NULL,
    Parent_id INT,
    PRIMARY KEY(id),
    FOREIGN KEY(POD_id)
		REFERENCES POD(id)
		ON UPDATE CASCADE
		ON DELETE RESTRICT
	);

CREATE TABLE IF NOT EXISTS Device (
	id INT NOT NULL AUTO_INCREMENT,
	Hostname VARCHAR(45) NOT NULL,
    Physisch ENUM('Ja', 'Nein'),
    Seriennummer VARCHAR(45) NOT NULL,
    DeviceType_id INT NOT NULL,
    Hersteller_id INT NOT NULL,
    IPInformationen_id INT NOT NULL,
    Location_id INT NOT NULL,
    PRIMARY KEY(id),
    FOREIGN KEY(DeviceType_id)
		REFERENCES DeviceType(id)
		ON UPDATE CASCADE
		ON DELETE RESTRICT,
    FOREIGN KEY(Hersteller_id)
		REFERENCES Hersteller(id)
		ON UPDATE CASCADE
		ON DELETE RESTRICT,
	FOREIGN KEY(IPInformationen_id)
		REFERENCES IPInformationen(id)
		ON UPDATE CASCADE
		ON DELETE RESTRICT,
    FOREIGN KEY(Location_id)
		REFERENCES Location(id)
		ON UPDATE CASCADE
		ON DELETE RESTRICT
    );
    
CREATE TABLE IF NOT EXISTS InterfaceInfo (
	id INT NOT NULL AUTO_INCREMENT,
    Duplexmode BOOLEAN,
    Physisch BOOLEAN,
    Medium ENUM ( 'LWL', 'Kupfer'),
    Speed INT,
    VlanId INT,
    ConnectedTo INT,
    PRIMARY KEY(id)
	) ;
    
CREATE TABLE IF NOT EXISTS DevicePorts (
	id INT NOT NULL AUTO_INCREMENT,
    portnr INT,
    active ENUM ('Y','N'),
	InterfaceInfo_id INT,
	Device_id INT NOT NULL,
	PRIMARY KEY (id),
    FOREIGN KEY(Device_id)
		REFERENCES Device(id)
		ON UPDATE CASCADE
		ON DELETE RESTRICT,
	FOREIGN KEY(InterfaceInfo_id)
		REFERENCES InterfaceInfo(id)
		ON UPDATE CASCADE
		ON DELETE RESTRICT
	);

CREATE TABLE IF NOT EXISTS Logging (
	id INT NOT NULL AUTO_INCREMENT,
	Datum DATETIME NOT NULL,
	LogLevel INT NOT NULL,
	LogMessage VARCHAR(45) NOT NULL,
    Device_id INT NOT NULL,
    Cleared INT NOT NULL,
	PRIMARY KEY (id),
    FOREIGN KEY(Device_id)
		REFERENCES Device(id)
		ON UPDATE CASCADE
		ON DELETE RESTRICT
	);

CREATE TABLE IF NOT EXISTS CreditalsDevice (
	id INT NOT NULL AUTO_INCREMENT,
    Device_id INT NOT NULL,
    Creditals_id INT NOT NULL,
    PRIMARY KEY(id),
    FOREIGN KEY(Device_id)
		REFERENCES Device(id)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
	FOREIGN KEY(Creditals_id)
		REFERENCES Creditals(id)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
	);
    
CREATE TABLE IF NOT EXISTS GlobaleInfo (
	id INT NOT NULL AUTO_INCREMENT,
    Zeitzone_id INT NOT NULL,
    POD_id INT NOT NULL,
    PRIMARY KEY (id),
    FOREIGN KEY(Zeitzone_id)
		REFERENCES Zeitzone(id)
		ON UPDATE CASCADE
		ON DELETE RESTRICT,
	FOREIGN KEY(POD_id)
		REFERENCES POD(id)
		ON UPDATE CASCADE
		ON DELETE RESTRICT
	);

CREATE TABLE IF NOT EXISTS GlobInfoNTPServer (
	id INT NOT NULL AUTO_INCREMENT,
    GlobaleInfo_id INT NOT NULL,
    NTPServer_id INT NOT NULL,
    Priorität INT NOT NULL,
    PRIMARY KEY (id),
    FOREIGN KEY(GlobaleInfo_id)
		REFERENCES GlobaleInfo(id)
		ON UPDATE CASCADE
		ON DELETE RESTRICT,
	FOREIGN KEY(NTPServer_id)
		REFERENCES NTPServer(id)
		ON UPDATE CASCADE
		ON DELETE RESTRICT
	);
    
CREATE TABLE IF NOT EXISTS Prioritaeten (
	id INT NOT NULL AUTO_INCREMENT,
    Kontaktperson_id INT NOT NULL,
    POD_id INT NOT NULL,
    Prioritaet INT NOT NULL,
    PRIMARY KEY(id),
    FOREIGN KEY (Kontaktperson_id)
		REFERENCES Kontaktperson(id)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
	FOREIGN KEY (POD_id)
		REFERENCES POD(id)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
	);

CREATE TABLE IF NOT EXISTS Adressart (
	Adresse_id INT NOT NULL,
    Location_id INT NOT NULL,
    Kundenkonto_id INT NOT NULL,
    Adresstyp ENUM ( 'Lieferadresse', 'Rechnungsadresse', 'Liefer- & Rechnungsadresse'),
    PRIMARY KEY(Adresse_id),
    FOREIGN KEY(Adresse_id)
		REFERENCES Adresse(id)
        ON UPDATE CASCADE
		ON DELETE RESTRICT,
    FOREIGN KEY(Location_id)
		REFERENCES Location(id)
        ON UPDATE CASCADE
		ON DELETE RESTRICT,
    FOREIGN KEY(Kundenkonto_id)
		REFERENCES Kundenkonto(id)
        ON UPDATE CASCADE
		ON DELETE RESTRICT
	);

CREATE TABLE IF NOT EXISTS Belege (
	id INT NOT NULL AUTO_INCREMENT,
	Kundenkonto_id INT,
	Datum DATETIME,
	Status ENUM('Offen', 'Bezahlt', 'Bezahlt mit Guthaben'),
	PRIMARY KEY(id),
	FOREIGN KEY(Kundenkonto_id)
		REFERENCES Kundenkonto(id)
		ON UPDATE CASCADE
		ON DELETE RESTRICT
	);

CREATE TABLE IF NOT EXISTS Produkte (
	id INT NOT NULL AUTO_INCREMENT,
	Beschreibung VARCHAR(45),
	Anzahl INT NOT NULL,
	Stuckpreis DECIMAL NOT NULL,
	Belege_id INT NOT NULL,
	Datum DATETIME,
	PRIMARY KEY(id),
	FOREIGN KEY(Belege_id)
		REFERENCES Belege(id)
		ON UPDATE CASCADE
		ON DELETE RESTRICT
	);

CREATE TABLE IF NOT EXISTS Standartleistungen (
	id INT NOT NULL AUTO_INCREMENT,
	Beschreibung VARCHAR(45) NOT NULL,
	Stundenansatz DECIMAL NOT NULL,
	AnzahlStunden DECIMAL NOT NULL,
	Belege_id INT NOT NULL,
	Datum DATETIME,
	PRIMARY KEY(id),
	FOREIGN KEY(Belege_id)
	REFERENCES Belege(id)
		ON UPDATE CASCADE
		ON DELETE RESTRICT
	);

# Views für Datenbank einfügen

# View für die Auswertung der Freien Interfaces pro Geraet
drop view if exists v_freePorts_perDevice;

CREATE ALGORITHM = MERGE VIEW v_freePorts_perDevice (Location, Hostname, Portnummer, Medium, Speed, Duplexmode)
AS
	SELECT Device.location_id, Device.hostname, DevicePorts.id, InterfaceInfo.Medium, InterfaceInfo.speed, InterfaceInfo.duplexmode
		FROM DevicePorts LEFT OUTER JOIN Device ON Device.id=DevicePorts.device_id
		LEFT OUTER JOIN InterfaceInfo ON DevicePorts.interfaceInfo_id=InterfaceInfo.id
        WHERE DevicePorts.active='N'
        ORDER BY Hostname;


# View für die Auswertung der Auslastung der Netzwerkschnittstellen pro Gerätekategorie pro Location
drop view if exists v_UsagePerLocation;

CREATE VIEW v_UsagePerLocation (location_id, DeviceType, Auslastung)
AS
	SELECT location_id, DeviceType, ((count(InterfaceInfo_id)/count(active))*100) as Auslastung
		FROM Device INNER JOIN DevicePorts ON Device.id=DevicePorts.device_id
					INNER JOIN DeviceType ON Device.DeviceType_id=DeviceType.id
                    INNER JOIN Location ON Device.Location_id=Location.id
        GROUP BY DeviceType_id;

        
# View für die Auswertung der Auslastung der Netzwerkschnittstellen pro Gerätekategorie pro POD
drop view if exists v_UsagePerPOD;

CREATE VIEW v_UsagePerPOD (POD, location_id, DeviceType, Auslastung)
AS
	SELECT POD.id, location_id, DeviceType, ((count(InterfaceInfo_id)/count(active))*100) as Auslastung
		FROM Device INNER JOIN DevicePorts ON Device.id=DevicePorts.device_id
					INNER JOIN DeviceType ON Device.DeviceType_id=DeviceType.id
                    INNER JOIN Location ON Device.Location_id=Location.id
                    INNER JOIN POD ON Location.POD_id=POD.id
        GROUP BY DeviceType_id;
 
 
# View für die Monitoring-Tools
drop view if exists v_logentries;

CREATE VIEW v_logentries(id, POD, Location, hostname, severity, timestamp, message)
AS
	SELECT Logging.id as id, POD.name as POD, Location.name as Location, Device.hostname as hostname, Logging.LogLevel as severity, Logging.Datum as timestamp, Logging.LogMessage as message
		FROM Device INNER JOIN Location ON Device.Location_id=Location.id
                    INNER JOIN POD ON Location.POD_id=POD.id
                    INNER JOIN Logging ON Device.id=Logging.Device_id
		WHERE Logging.Cleared=0
                    ;

# View mit allen Belegen
drop view if exists v_belege;

CREATE VIEW v_belege(id, Kundennummer, Firmenname, Status, Datum, Total)
AS
	Select Positionen.Belege_id as id, Positionen.Kundenkonto_id as Kundennummer, Kunde.Firmenname as Firmenname, Positionen.Status as Status, Positionen.Datum as Datum, SUM(Positionen.TotalPosition) as Total 
		FROM (
			SELECT Belege_id, Kundenkonto_id, Beschreibung, Belege.Status as Status, Anzahl*Stuckpreis as TotalPosition, Belege.Datum as Datum
				FROM Produkte LEFT OUTER JOIN Belege on Belege.id=Produkte.Belege_id
			UNION ALL
			SELECT Belege_id, Kundenkonto_id, Beschreibung, Belege.Status as Status, AnzahlStunden*Stundenansatz as TotalPosition, Belege.Datum as Datum
				FROM Standartleistungen
					LEFT OUTER JOIN Belege on Belege.id=Standartleistungen.Belege_id
		) as Positionen
			LEFT JOIN Kunde ON Kunde.KundenNr=Kundenkonto_id
		GROUP BY Positionen.Belege_id;

# Stored Procedures zu Datenbank hinzfügen

# Hinzfügen von Log Nachrichten
drop procedure if exists LogMessageAdd;

DELIMITER //

CREATE PROCEDURE LogMessageAdd (
IN date datetime,
IN host VARCHAR(255),
IN LogLev INT,
IN Message VARCHAR(255)
)

proc: BEGIN
SELECT id
	FROM Device
    WHERE hostname=host
    GROUP BY hostname
    INTO @devnr;

IF @devnr IS NOT NULL THEN
	INSERT INTO Logging (datum, loglevel, logmessage, device_id, Cleared)
    VALUES (date, loglev, message, @devnr, 0);
    ELSE
    LEAVE proc;
    END IF; 

END //

DELIMITER ;

# Hinzufügen von Geräten

drop procedure if exists DeviceAdd;
DELIMITER //

CREATE PROCEDURE DeviceAdd (
IN host VARCHAR(255),
IN physic ENUM('Ja', 'Nein'),
IN serial VARCHAR(255),
IN devtype VARCHAR(255),
IN herst VARCHAR(255),
IN ipinfo INT,
IN loca VARCHAR(255),
IN pod VARCHAR(255),
IN ports INT
)

proc: BEGIN

DECLARE cont INT UNSIGNED DEFAULT 0;
DECLARE lastdev INT UNSIGNED DEFAULT 0;
DECLARE portnum INT UNSIGNED DEFAULT 1;

SELECT id
	FROM POD
    WHERE name=POD
    INTO @podnr;

SELECT id
	FROM DeviceType
    WHERE DeviceType=devtype
    GROUP BY DeviceType
    INTO @devnr;
    
SELECT id
	FROM Hersteller
    WHERE Herstellername=herst
    GROUP BY Herstellername
    INTO @supplier;
    
SELECT id
	FROM Location
    WHERE name=loca AND pod_id=@podnr
    INTO @loc;
    
INSERT INTO Device (hostname, physisch, seriennummer, devicetype_id, hersteller_id, ipinformationen_id, location_id)
	VALUES	(host, physic, serial, @devnr, @supplier, ipinfo, @loc);
    
SET lastdev=LAST_INSERT_ID();

WHILE cont<=ports DO
	INSERT INTO InterfaceInfo (id, Duplexmode, Physisch, Medium, Speed, VlanId, ConnectedTo)
	VALUES	(NULL, NULL, NULL, NULL, NULL, NULL, NULL);
    INSERT INTO DevicePorts (active, InterfaceInfo_id, Device_id, portnr)
	VALUES ('N', last_insert_id(), lastdev, portnum);
    SET cont = cont + 1;
    set portnum = portnum + 1;
    END WHILE;

END //

DELIMITER ;

# Interface von Geräten aktivieren

drop procedure if exists ActivateInterface;
DELIMITER //

CREATE PROCEDURE ActivateInterface (
IN host VARCHAR(255),
IN loca VARCHAR(255),
IN port INT,
IN pod VARCHAR(255),
IN Duplex INT,
IN physic INT,
IN medi ENUM( 'LWL', 'Kupfer'),
IN tempo INT,
IN vlan INT,
IN connect VARCHAR(255),
IN conloc VARCHAR(255),
IN conport INT
)


proc: BEGIN

DECLARE pod1 INT UNSIGNED DEFAULT 0;
DECLARE loc1 INT UNSIGNED DEFAULT 0;
DECLARE loc2 INT UNSIGNED DEFAULT 0;
DECLARE dev1 INT UNSIGNED DEFAULT 0;
DECLARE dev2 INT UNSIGNED DEFAULT 0;
DECLARE ok1 INT UNSIGNED DEFAULT 0;
DECLARE ok2 INT UNSIGNED DEFAULT 0;
DECLARE port1 INT UNSIGNED DEFAULT 0;
DECLARE port2 INT UNSIGNED DEFAULT 0;
DECLARE id1 INT UNSIGNED DEFAULT 0;
DECLARE id2 INT UNSIGNED DEFAULT 0;
DECLARE id3 INT UNSIGNED DEFAULT 0;
DECLARE id4 INT UNSIGNED DEFAULT 0;

SET pod1=pod;

SELECT Location.id, Device.id
	FROM Device INNER JOIN Location ON Device.Location_id
				INNER JOIN POD ON POD_id
    WHERE Location.name=loca AND Device.Hostname=host AND Location.POD_id=pod1
    GROUP BY Device.hostname
    INTO loc1, dev1;

SELECT Location.id, Device.id
	FROM Device INNER JOIN Location ON Device.Location_id
				INNER JOIN POD ON POD_id
    WHERE Location.name=conloc AND Device.Hostname=connect AND Location.POD_id=pod1
    GROUP BY Device.hostname
    INTO loc2, dev2;

SELECT id, InterfaceInfo_id
	FROM DevicePorts
    WHERE device_id = dev1 AND portnr = port
    INTO port1, id3;
    
SELECT id, InterfaceInfo_id
	FROM DevicePorts
    WHERE device_id = dev2 AND portnr = conport
    INTO port2, id4;

IF physic=1  THEN       

UPDATE InterfaceInfo
	SET Duplexmode = Duplex,
    Physisch = physic,
    Medium =medi,
    Speed = tempo,
    VlanId = vlan,
    ConnectedTo = port2
    WHERE id = id3;
    
UPDATE InterfaceInfo
	SET Duplexmode = Duplex,
    Physisch = physic,
    Medium =medi,
    Speed = tempo,
    VlanId = vlan,
    ConnectedTo = port2
    WHERE id = id4;
    
ELSE
UPDATE InterfaceInfo
	SET Duplexmode = Duplex,
    Physisch = physic,
    Medium =medi,
    Speed = tempo,
    VlanId = vlan,
    ConnectedTo = port2
    WHERE id = id3;
END IF;

END //

DELIMITER ;

# Logeintrag quitieren
drop procedure if exists LogClear;

DELIMITER //

CREATE PROCEDURE LogClear (
IN id INT,
OUT total INT
)

proc: BEGIN
UPDATE Logging 
	SET
		cleared=1
        WHERE Logging.id=id;
        
SELECT ROW_COUNT() INTO total;

END //

DELIMITER ;

# Überpfrügun der Belege von einer POD
drop procedure if exists PodBill;

DELIMITER //

CREATE PROCEDURE PodBill (
	ID INT
)

proc: BEGIN
	DECLARE cur_beleg_id, cur_customer_id, var_done INT DEFAULT 0;
	DECLARE myCursor CURSOR FOR
		SELECT v_belege.id, v_belege.Kundennummer
			FROM v_belege
			WHERE Kundennummer = ID;
	DECLARE CONTINUE HANDLER FOR NOT FOUND SET var_done = 1; SET var_done = 0;
	OPEN myCursor;
	FETCH NEXT FROM myCursor INTO cur_beleg_id, cur_customer_id;
	WHILE NOT var_done DO
		UPDATE Belege 
			SET Belege.Status = 'Bezahlt mit Guthaben'
			WHERE Belege.id = cur_beleg_id;
		SET var_done = 0;
		FETCH myCursor INTO cur_beleg_id, cur_customer_id;
	END WHILE;
	CLOSE myCursor;
END //

DELIMITER ;

# Überpfrügun aller Belege auf Limit des Kunden
drop procedure if exists CheckBelegeLimit;

DELIMITER //

CREATE PROCEDURE CheckBelegeLimit (
	LimitWert INT
)

proc: BEGIN
	DECLARE cur_beleg_id, var_done INT DEFAULT 0;
	DECLARE myCursor CURSOR FOR
		SELECT v_belege.id
			FROM v_belege
			WHERE v_belege.Status = 'Offen' AND v_belege.Total > LimitWert;
	DECLARE CONTINUE HANDLER FOR NOT FOUND SET var_done = 1; SET var_done = 0;
	OPEN myCursor;
	FETCH NEXT FROM myCursor INTO cur_beleg_id;
	WHILE NOT var_done DO
		UPDATE Belege 
			SET Belege.Status = 'Bezahlt mit Guthaben'
			WHERE Belege.id = cur_beleg_id;
		SET var_done = 0;
		FETCH myCursor INTO cur_beleg_id;
	END WHILE;
	CLOSE myCursor;
END //

DELIMITER ;
    
# Überpfrügun aller Belege Ende Monat
drop procedure if exists CheckBelegeMonat;

DELIMITER //

CREATE PROCEDURE CheckBelegeMonat ()

proc: BEGIN
	DECLARE cur_beleg_id, var_done INT DEFAULT 0;
	DECLARE myCursor CURSOR FOR
		SELECT v_belege.id
			FROM v_belege
			WHERE v_belege.Status = 'Offen';
	DECLARE CONTINUE HANDLER FOR NOT FOUND SET var_done = 1; SET var_done = 0;
	OPEN myCursor;
	FETCH NEXT FROM myCursor INTO cur_beleg_id;
	WHILE NOT var_done DO
		UPDATE Belege 
			SET Belege.Status = 'Bezahlt mit Guthaben'
			WHERE Belege.id = cur_beleg_id;
		SET var_done = 0;
		FETCH myCursor INTO cur_beleg_id;
	END WHILE;
	CLOSE myCursor;
END //

DELIMITER ;

# Überpfrügun aller Belege eine Position älter als 3 Monate
drop procedure if exists CheckBelegeEndMonth;

DELIMITER //

CREATE PROCEDURE CheckBelegeEndMonth ()

proc: BEGIN
	DECLARE cur_beleg_id, var_done INT DEFAULT 0;
	DECLARE myCursor CURSOR FOR
		Select Positionen.Belege_id
		FROM (
			SELECT Belege_id, Kundenkonto_id, Beschreibung, Belege.Status as Status, Anzahl*Stuckpreis as TotalPosition, Belege.Datum as Datum
				FROM Produkte
					LEFT OUTER JOIN Belege on Belege.id=Produkte.Belege_id
				WHERE Belege.Status = 'Offen' AND Belege.Datum < DATE_SUB(NOW(), INTERVAL 3 MONTH)
			UNION ALL
			SELECT Belege_id, Kundenkonto_id, Beschreibung, Belege.Status as Status, AnzahlStunden*Stundenansatz as TotalPosition, Belege.Datum as Datum
				FROM Standartleistungen
					LEFT OUTER JOIN Belege on Belege.id=Standartleistungen.Belege_id
				WHERE Belege.Status = 'Offen' AND Belege.Datum < DATE_SUB(NOW(), INTERVAL 3 MONTH)
		) as Positionen;
	DECLARE CONTINUE HANDLER FOR NOT FOUND SET var_done = 1; SET var_done = 0;
	OPEN myCursor;
	FETCH NEXT FROM myCursor INTO cur_beleg_id;
	WHILE NOT var_done DO
		UPDATE Belege 
			SET Belege.Status = 'Bezahlt mit Guthaben'
			WHERE Belege.id = cur_beleg_id;
		SET var_done = 0;
		FETCH myCursor INTO cur_beleg_id;
	END WHILE;
	CLOSE myCursor;
END //

DELIMITER ;

# trigger sobald belege hinzugefügt werden
DROP TRIGGER IF EXISTS BelegeCheckTrigger1;

DELIMITER //

CREATE TRIGGER BelegeCheckTrigger1 AFTER INSERT ON Belege
	CALL CheckBelegeLimit('10000');
    
DELIMITER ;
DROP TRIGGER IF EXISTS BelegeCheckTrigger2;

DELIMITER //

CREATE TRIGGER BelegeCheckTrigger2 AFTER INSERT ON Belege
	CALL CheckBelegeMonat();
    
DELIMITER ;
DROP TRIGGER IF EXISTS BelegeCheckTrigger3;

DELIMITER //

CREATE TRIGGER BelegeCheckTrigger3 AFTER INSERT ON Belege
	CALL CheckBelegeEndMonth();
    
DELIMITER ;
  
  /*CREATE TABLE IF NOT EXISTS*/

/*
Name:         Semesterprojekt
Description:  Datenbank mit Testdaten befüllen
*/

INSERT INTO `NTPServer` (`id`, `Servername`, `IPAdresse`)
	VALUES 	(NULL, 'ntp.metas.ch', '162.23.41.10'),
			(NULL, '0.ch.pool.ntp.org', '81.94.123.17');
INSERT INTO `Zeitzone` (`id`, `Zonenname`, `Zeitabweichung`)
	VALUES 	(NULL, 'Europa/Zuerich', '1'),
			(NULL, 'Europa/Island', '0');

INSERT INTO `Ort` (`id`, `Plz`, `Ort`)
	VALUES 	(1, '9000', 'St. Gallen'),
			(2, '9100', 'Herisau'),
            (3, '9200', 'Gossau'),
            (4, '9050', 'Appenzell'),
            (5, '9500', 'Wil'),
            (6, '9030', 'Flawil');
INSERT INTO `Adresse` (`id`, `Strasse`, `Nummer`, `Ort_id`)
	VALUES 	(1, 'Rushstrasse', '9', '1'),
			(2, 'Töbeliweg', '13', '3'),
            (3, 'Säntisstrasse', '2', '2'),
            (4, 'Gontnerstrasse', '44', '4'),
            (5, 'Zürcherstrasse', '23', '5'),
            (6, 'Uzwilerstrasse', '108', '6');

INSERT INTO `Kunde` (`KundenNr`, `Firmenname`)
	VALUES 	(NULL, 'Nöldi AG'),
			(NULL, 'Global Consulting');
INSERT INTO `Kontaktperson` (`id`, `Nachname`, `Vorname`)
	VALUES 	(NULL, 'Spencer', 'Bud'),
			(NULL, 'Hill', 'Terence');
INSERT INTO `Telefonnummer` (`id`, `Kontaktperson_id`, `Name`, `Art`, `Nummer`)
	VALUES 	(NULL, '2', 'Zentrale', 'Geschäft', '+417142424242'),
			(NULL, '2', 'Picket-telefon', 'Mobile', '+417942424242'),
			(NULL, '1', 'Handy', 'Mobile', '');

INSERT INTO `Zahlungen` (`id`, `Betrag`, `Zahlungseingang`)
	VALUES 	(NULL, '24000.99', '2018-08-15'),
			(NULL, '3900.82', '2018-09-01');
INSERT INTO `Kundenkonto` (`id`, `KundenNr_id`, `Zahlungen_id`)
	VALUES 	(NULL, '1', '2'),
			(NULL, '2', '1');
INSERT INTO `POD` (`id`, `name`, `Kundenkonto_id`)
	VALUES 	(NULL,'Nöldi Gmbh', '1'),
			(NULL,'Swiss Consulting', '2');
INSERT INTO `Location` (`id`, `name`, `POD_id`,`Parent_id`)
	VALUES 	(1,'Schweiz', '1',0),
            (2,'Hauptsitz', '1',1),
			(3,'Filiale Gossau', '1',2),
			(4,'Agentur Flawil', '1',3),
            (5,'Hauptsitz', '2',1),
            (6,'Filiale Appenzell', '2',5),
            (7,'Filiale Herisau','2',5);
INSERT INTO `Prioritaeten` (`id`, `Kontaktperson_id`, `POD_id`, `Prioritaet`)
	VALUES 	(NULL, '2', '1', '1'),
			(NULL, '1', '2', '1');
INSERT INTO `Adressart` (`Adresse_id`, `Location_id`, `Kundenkonto_id`, `Adresstyp`)
	VALUES 	('1', '1', '1', 'Lieferadresse'),
			('2', '2', '1', 'Liefer- & Rechnungsadresse'),
            ('3', '3', '1', 'Liefer- & Rechnungsadresse'),
            ('4', '4', '2', 'Lieferadresse'),
            ('5', '5', '2', 'Liefer- & Rechnungsadresse');
INSERT INTO `Belege` (`id`, `Kundenkonto_id`, `Datum`, `Status`)
	VALUES
		(NULL, '1', '2018-08-15', 'Offen'),
		(NULL, '2', '2018-08-15', 'Offen');

INSERT INTO `Produkte` (`id`, `Beschreibung`, `Anzahl`, `Stuckpreis`, `Datum`, `Belege_id`)
	VALUES 	(NULL, 'Switch DELL 24 Ports', '2', '500', '2018-08-15', '2'),
			(NULL, 'Switch DELL 12 Ports', '2', '350', '2018-08-15', '1');
INSERT INTO `Standartleistungen` (`id`, `Beschreibung`, `Stundenansatz`, `AnzahlStunden`, `Datum`, `Belege_id`)
	VALUES 	(NULL, 'Support', '250', '10', '2018-08-15', '1'),
			(NULL, 'Support 24/7', '300', '2', '2018-08-15', '2');

INSERT INTO `GlobaleInfo` (`id`, `Zeitzone_id`, `POD_id`)
	VALUES 	(NULL, '2', '1'),
			(NULL, '1', '2');
INSERT INTO `GlobInfoNTPServer` (`id`, `GlobaleInfo_id`, `NTPServer_id`, `Priorität`)
	VALUES 	(NULL, '1', '1', '1'),
			(NULL, '2', '2', '1');

INSERT INTO `IPInformationen` (`id`, `IPAdresse`, `Subnetmaske`, `Gateway`)
	VALUES 	(NULL, '192.168.1.10', '255.255.255.0', '192.168.1.1'),
			(NULL, '10.12.69.10', '255.255.252.0', '10.12.68.2'),
			(NULL, '12.10.72.5', '255.255.255.0', '12.10.72.2');
INSERT INTO `DeviceType` (`id`, `DeviceType`)
	VALUES 	(NULL, 'Switch'),
			(NULL, 'Router'),
            (NULL, 'Gateway'),
            (NULL, 'Drucker'),
            (NULL, 'Server'),
            (NULL, 'Computer');
INSERT INTO `Hersteller` (`id`, `Herstellername`)
	VALUES 	(NULL, 'Cisco'),
			(NULL, 'Dell'),
            (NULL, 'Sophos'),
            (NULL, 'Canon'),
            (NULL, 'HP'),
            (NULL, 'Microsoft'),
            (NULL, 'RedHat');


# Device mittels Stored Procedure hinzufügen
# (hostname, physisch, seriennummer, device type, Hersteller, ip information, location, podname, anzahl ports)        
CALL DeviceAdd ('DVC002',  'Ja', 'StPKZte5', 'Switch', 'Cisco', '1', 'Hauptsitz', 'Nöldi Gmbh', 24);
CALL DeviceAdd ('DVC001', 'Ja', 'TVDxYKCD', 'Gateway', 'Sophos', '1', 'Hauptsitz', 'Nöldi Gmbh', 4);
CALL DeviceAdd ('DVC003', 'Ja', 'YKCDxTVD', 'Gateway', 'Sophos', '2', 'Filiale Gossau', 'Nöldi Gmbh', 4);
CALL DeviceAdd ('DVC004', 'Ja', 'YergDxTVD', 'Switch', 'Cisco', '2', 'Filiale Gossau', 'Nöldi Gmbh', 16);
CALL DeviceAdd ('imageRUNNER ADVANCE C256', 'Ja', '26642efje', 'Drucker', 'Canon', '2', 'Filiale Gossau', 'Nöldi Gmbh', 1);
CALL DeviceAdd ('Ghost002', 'Ja', '7mH5RfFm', 'Router', 'Dell', '3', 'Hauptsitz', 'Swiss Consulting', 4);
CALL DeviceAdd ('ESXI002', 'Ja', 'rui45m6p2', 'Server', 'Dell', '1', 'Hauptsitz', 'Nöldi Gmbh', 4);
CALL DeviceAdd ('SRV001', 'Nein', 'rui45m6p2', 'Server', 'Microsoft', '1', 'Hauptsitz', 'Nöldi Gmbh', 1);
CALL DeviceAdd ('SRV002', 'Nein', 'rui45m4p2', 'Server', 'RedHat', '1', 'Hauptsitz', 'Nöldi Gmbh', 4);
CALL DeviceAdd ('HSW001', 'Ja', '345gaew', 'Computer', 'HP', '3', 'Hauptsitz', 'Swiss Consulting', 1);
CALL DeviceAdd ('HSW002', 'Ja', 'erftzh65', 'Computer', 'Dell', '3', 'Hauptsitz', 'Nöldi Gmbh', 1);
CALL DeviceAdd ('HSW001', 'Ja', 'wergsw', 'Computer', 'Dell', '1', 'Hauptsitz', 'Nöldi Gmbh', 1);
CALL DeviceAdd ('FGW001', 'Ja', 'agwewwq', 'Computer', 'HP', '2', 'Filiale Gossau', 'Nöldi Gmbh', 1);
CALL DeviceAdd ('DVC001', 'Ja', 'ffetN567', 'Switch', 'HP', '2', 'Hauptsitz', 'Swiss Consulting', 16);

# Interface mittels Stored Procedure aktivieren
# (hostname, location, port, pod, duplex modus, physisch, medium, verbindungs tempo, vlan-id, connected device, location des device, port des connected device)        
CALL ActivateInterface('DVC002', 'Hauptsitz', 1, 1, 1, 1, 'LWL', 10000, 1, 'DVC001', 'Hauptsitz', 2);
CALL ActivateInterface('DVC001', 'Filiale Gossau', 2, 1, 1, 0, 'Kupfer', 1000, 1, NULL, NULL, NULL);
CALL ActivateInterface('DVC004', 'Filiale Gossau', 5, 1, 1, 1, 'Kupfer', 1000, 2, 'imageRUNNER ADVANCE C256', 'Filiale Gossau', 1);
CALL ActivateInterface('DVC004', 'Filiale Gossau', 1, 1, 1, 1, 'Kupfer', 1000, 1, 'DVC003', 'Filiale Gossau', 1);
CALL ActivateInterface('DVC003', 'Filiale Gossau', 2, 1, 1, 0, 'Kupfer', 1000, 1, NULL, NULL, NULL);
CALL ActivateInterface('ESXI002', 'Hauptsitz', 2, 1, 1, 1, 'Kupfer', 10000, 3, 'DVC002', 'Hauptsitz', 2);
CALL ActivateInterface('ESXI002', 'Hauptsitz', 2, 1, 1, 1, 'Kupfer', 10000, 3, 'DVC002', 'Hauptsitz', 3);
CALL ActivateInterface('ESXI002', 'Hauptsitz', 2, 1, 1, 1, 'Kupfer', 10000, 3, 'DVC002', 'Hauptsitz', 4);
CALL ActivateInterface('HSW001', 'Hauptsitz', 1, 1, 1, 1, 'Kupfer', 1000, 4, 'DVC002', 'Hauptsitz', 24);
CALL ActivateInterface('HSW001', 'Hauptsitz', 1, 1, 1, 1, 'Kupfer', 1000, 4, 'DVC002', 'Hauptsitz', 23);
CALL ActivateInterface('FGW001', 'Filiale Gossau', 2, 1, 1, 1, 'Kupfer', 1000, 3, 'DVC004', 'Filiale Gossau', 16);
CALL ActivateInterface('HSW001', 'Hauptsitz', 2, 2, 1, 1, 'Kupfer', 1000, 3, 'DVC001', 'Hauptsitz', 16);
CALL ActivateInterface('SRV001', 'Hauptsitz', 1, 1, 1, 0, 'Kupfer', 1000, 1, NULL, NULL, NULL);
CALL ActivateInterface('SRV002', 'Hauptsitz', 1, 1, 1, 0, 'Kupfer', 1000, 1, NULL, NULL, NULL);
         

INSERT INTO `Logging` (`id`, `Datum`, `LogLevel`, `LogMessage`, `Device_id`, `Cleared`)
	VALUES 	(NULL, '2018-08-05 01:00:00', '1', 'Hello World', '1', 0),
			(NULL, '2018-09-11 07:00:26', '1', 'Hello args', '2', 0),
            (NULL, '2018-08-08 00:01:00', '100', 'ERROR CODE: 42', '3', 0),
            (NULL, '2018-08-08 00:01:00', '100', 'Alles am Arsch...', '10', 0);


INSERT INTO `Creditals` (`id`, `Benutzername`, `Passwort`, `SNMPCommunity`)
	VALUES 	(NULL, 'root', 'hihohu42', 'root'),
			(NULL, 'chuck', 'NorrisBest99', 'norris');
INSERT INTO `CreditalsDevice` (`id`, `Device_id`, `Creditals_id`)
	VALUES 	(NULL, '1', '2'),
			(NULL, '2', '2'),
            (NULL, '3', '1');
