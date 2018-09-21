CREATE DATABASE fp;

\c fp; 

CREATE TABLE addresses
(
    address_id character varying(250) primary key,
    address_street character varying(250),
    address_zipcode character varying(25),
    address_phone character varying(250)
);

CREATE TABLE users
(
    user_id character varying(250) primary key,
    user_email character varying(250),
    user_password character varying(250),
    address_id character varying(250) references addresses (address_id)
);


insert into addresses(address_id, address_street, address_zipcode, address_phone) values('address_1', 'street 1', 'zipcode 1', '+40 123 454 678');


insert into users(user_id, user_email, user_password, address_id) values('1', 'test1@example.com', 'password1', 'address_1');
insert into users(user_id, user_email, user_password, address_id) values('2', 'test2@example.com', 'password1', 'address_1');