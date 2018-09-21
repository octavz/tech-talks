CREATE DATABASE fp;

\c fp; 

CREATE TABLE users
(
    user_id character varying(250) primary key,
    user_email character varying(250),
    user_password character varying(250)
);



insert into users(user_id, user_email, user_password) values('1', 'test1@example.com', 'password1');
insert into users(user_id, user_email, user_password) values('2', 'test2@example.com', 'password1');