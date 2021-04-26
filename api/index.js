const express = require("express");
const path = require("path");

const app = express();
const PORT = process.env.PORT || "8000";
const lambda = !!process.env.LAMBDA_TASK_ROOT;

const AWS = require('aws-sdk');
AWS.config.update({
    region: 'us-east-1',
    accessKeyId: 'AKIASO3OHQ4WQC3Z4OED',
    secretAccessKey: '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'
});

let dynamodb = new AWS.DynamoDB();

app.use(express.json())

app.get("/", (req, res) => {
    res.status(200).send("<b>GME Project 2 API is Online</b>");
});

app.get("/user", async (req, res) => {
    console.log("\nGET /user");
    const { name } = req.query;
    if ( !name ) res.status(400).send({ message: 'Error 400 - Bad Request'});
    
    const params = { TableName: "GME-Project-2-Table", Key: { Name: { S: name } } };
    let request = dynamodb.getItem(params);
    await request.promise()
    .then(data => {
        if ( !data.Item ) {
            console.log(`=== User "${name}" does not exist. Bad request!`);
            res.status(400).send({ message: 'Error 400 - Bad Request'});
            return;
        }

        console.log(`=== User "${name}" successfully retrieved!`);
        console.log("=== DB Entry =", data.Item)
        const item = AWS.DynamoDB.Converter.unmarshall(data.Item);
        console.log("===", item)
        res.status(200).send(item);
    })
    .catch(err => {
        console.log(err);
        console.log("=== Error invoking DynamoDB GetItem!")
        res.status(500).send({ message: "Error 500 - Server Error" });
    });
});

app.post("/user", async (req, res) => {
    console.log("\nPOST /user");
    console.log("=== Body: ", req.body)
    const { name, score } = req.body;
    if ( !(name && score) ) res.status(400).send({ message: "Error 400 - Bad Request" });
    
    let params = { TableName: "GME-Project-2-Table", Key: { Name: { S: name } } };
    let request = dynamodb.getItem(params);
    await request.promise()
    .then(async data => {
        if ( !data.Item ) throw "User does not exist";
        console.log(`=== User "${name}" exists. Updating user...`);
        console.log("=== Current DB Entry: ", data.Item)
        
        params = {
            TableName: "GME-Project-2-Table",
            Key: { Name: { S: name } },
            ExpressionAttributeValues: { ":score": { S: String(score) } },
            UpdateExpression: "SET Score = :score"
        };
        let request = dynamodb.updateItem(params);
        await request.promise()
        .then(data => {
            console.log(`=== User "${name}" has been successfully updated!`)
            res.status(200).send({ message: `User "${name}" has been successfully updated!` });
        })
        .catch(err => {
            console.log(err);
            console.log("=== Error invoking DynamoDB UpdateItem!")
            res.status(500).send({ message: "Error 500 - Server Error" });
        });
    })
    .catch(async err => {
        console.log(`=== User "${name}" does not exist. Creating user...`);

        const item = AWS.DynamoDB.Converter.marshall({ Name: name, Score: String(score) });
        params = { TableName: "GME-Project-2-Table", Item: item };
        let request = dynamodb.putItem(params);
        await request.promise()
        .then(data => {
            console.log(`=== User "${name}" has been successfully updated!`)
            res.status(200).send({ message: `User "${name}" has been successfully created!` });
        })
        .catch(err => {
            console.log(err);
            console.log("=== Error invoking DynamoDB PutItem!")
            res.status(500).send({ message: "Error 500 - Server Error" });
        });
    });
});

if (lambda) {
    const serverlessExpress = require('aws-serverless-express');
    const server = serverlessExpress.createServer(app);
    exports.handler = (event, context) => serverlessExpress.proxy(server, event, context);
} else {
    app.listen(PORT, () => {
        console.log('\n=== GME Project 2 API is NOT running on AWS Lambda');
        console.log(`=== GME Project 2 API is listening at http://localhost:${PORT}`);
    });
}