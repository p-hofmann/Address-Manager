CREATE TABLE country (
	id INTEGER PRIMARY KEY,
	country_code INTEGER UNIQUE,
   	country_name TEXT NOT NULL
);

CREATE TABLE city (
	id INTEGER PRIMARY KEY,
	city_code INTEGER UNIQUE,
   	city_name TEXT NOT NULL,
	id_country INTEGER,
   FOREIGN KEY (id_country) 
      REFERENCES country (id) 
         ON DELETE CASCADE 
         ON UPDATE NO ACTION
);

CREATE TABLE street (
	id INTEGER PRIMARY KEY,
	name TEXT NOT NULL,
   	id_city INTEGER,
   FOREIGN KEY (id_city) 
      REFERENCES city (id) 
         ON DELETE CASCADE 
         ON UPDATE NO ACTION
);

CREATE TABLE address (
	id INTEGER PRIMARY KEY,
   	id_street INTEGER,
   	house_number INTEGER,
   FOREIGN KEY (id_street) 
      REFERENCES street (id) 
         ON DELETE CASCADE 
         ON UPDATE NO ACTION
);

CREATE TABLE person (
	id INTEGER PRIMARY KEY,
   	name_first TEXT NOT NULL,
	name_last TEXT NOT NULL,
	id_adress INTEGER,
   FOREIGN KEY (id_adress) 
      REFERENCES adress (id) 
         ON DELETE CASCADE 
         ON UPDATE NO ACTION
);

CREATE TABLE phone_category (
	id INTEGER PRIMARY KEY,
   	name TEXT UNIQUE NOT NULL
);

CREATE TABLE phone_number (
	id INTEGER PRIMARY KEY,
	number TEXT,
   	id_phone_category INTEGER,
   FOREIGN KEY (id_phone_category) 
      REFERENCES phone_category (id) 
         ON DELETE CASCADE 
         ON UPDATE NO ACTION
);

CREATE TABLE list_phone(
   id_phone_number INTEGER,
   id_person INTEGER,
   PRIMARY KEY (id_phone_number, id_person),
   FOREIGN KEY (id_phone_number) 
      REFERENCES phone_number (id) 
         ON DELETE CASCADE 
         ON UPDATE NO ACTION,
   FOREIGN KEY (id_person) 
      REFERENCES person (id) 
         ON DELETE CASCADE 
         ON UPDATE NO ACTION
);

CREATE TABLE picture (
	id INTEGER PRIMARY KEY,
	name TEXT NOT NULL,
   	data BLOB NOT NULL
);

CREATE TABLE list_picture(
   id_picture INTEGER,
   id_person INTEGER,
   PRIMARY KEY (id_picture, id_person),
   FOREIGN KEY (id_picture) 
      REFERENCES picture (id) 
         ON DELETE CASCADE 
         ON UPDATE NO ACTION,
   FOREIGN KEY (id_person) 
      REFERENCES person (id) 
         ON DELETE CASCADE 
         ON UPDATE NO ACTION
);
