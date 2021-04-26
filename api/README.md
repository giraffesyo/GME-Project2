# Immersive Computing Project 2 - Virtual Reality Cooking Game Back-End API

## Purpose

Express API to handle user score updates for Virtual Reality Cooking Game.

## Usage Guide

API is currently served on: https://fsxg5i1hzl.execute-api.us-east-1.amazonaws.com/prod <br>
*Resources & Methods:*
- GET / :: index page
- GET /user?name={USERNAME} :: Get User {USERNAME}
- POST /user :: Body = { "Name": {USERNAME}, "Score" {SCORE} } Post/Update User {USERNAME} in Table. 

## Installation Guide

Navigate to "api" folder in Terminal. Run `npm i` to install the dependencies. Run `node .` to start the application.

## Dependencies

- Node.js (14.x)
- AWS-SDK
- Express
- AWS-Serverless-Express