CREATE TABLE country (
  id INTEGER PRIMARY KEY NOT NULL,
  country_code INTEGER UNIQUE,
  country_name TEXT NOT NULL
);

CREATE TABLE city (
  id INTEGER PRIMARY KEY NOT NULL,
  postal_code TEXT UNIQUE,
  city_name TEXT NOT NULL,
  id_country INTEGER,
    FOREIGN KEY (id_country) 
      REFERENCES country (id) 
        ON DELETE NO ACTION 
        ON UPDATE NO ACTION
);

CREATE TABLE street (
	id INTEGER PRIMARY KEY NOT NULL,
	name TEXT NOT NULL,
  id_city INTEGER,
  FOREIGN KEY (id_city) 
    REFERENCES city (id) 
      ON DELETE NO ACTION 
      ON UPDATE NO ACTION
);

CREATE TABLE address (
	id INTEGER PRIMARY KEY NOT NULL,
  id_street INTEGER,
  id_city INTEGER,
  house_number TEXT,
  FOREIGN KEY (id_street) 
    REFERENCES street (id) 
      ON DELETE CASCADE 
      ON UPDATE NO ACTION,
  FOREIGN KEY (id_city) 
    REFERENCES city (id) 
      ON DELETE NO ACTION 
      ON UPDATE NO ACTION
);

CREATE TABLE person (
	id INTEGER PRIMARY KEY NOT NULL,
  name_first TEXT NOT NULL,
	name_last TEXT NOT NULL,
	id_address INTEGER,
  FOREIGN KEY (id_address) 
    REFERENCES address (id) 
      ON DELETE CASCADE 
      ON UPDATE NO ACTION
);

CREATE TABLE phone_category (
	id INTEGER PRIMARY KEY NOT NULL,
  name TEXT UNIQUE NOT NULL
);

CREATE TABLE phone_number (
  id INTEGER PRIMARY KEY NOT NULL,
  pnumber TEXT,
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
  id INTEGER PRIMARY KEY NOT NULL,
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
