import type { NextApiRequest, NextApiResponse } from 'next'

const AWS = require('aws-sdk')

const region = process.env.MOON_REGION
const accessKeyId = process.env.MOON_ACCESS_KEY_ID
const secretAccessKey = process.env.MOON_SECRET_ACCESS_KEY

AWS.config.update({
  region: region,
  accessKeyId: accessKeyId,
  secretAccessKey: secretAccessKey,
})

let dynamodb = new AWS.DynamoDB()

const GET = async (req: NextApiRequest, res: NextApiResponse) => {
  console.log('\nGET /user')
  const { name } = req.query
  if (!name) res.status(400).send({ message: 'Error 400 - Bad Request' })

  const params = {
    TableName: 'GME-Project-2-Table',
    Key: { Name: { S: name } },
  }
  let request = dynamodb.getItem(params)
  await request
    .promise()
    .then((data) => {
      if (!data.Item) {
        console.log(`=== User "${name}" does not exist. Bad request!`)
        res.status(400).send({ message: 'Error 400 - Bad Request' })
        return
      }

      console.log(`=== User "${name}" successfully retrieved!`)
      console.log('=== DB Entry =', data.Item)
      const item = AWS.DynamoDB.Converter.unmarshall(data.Item)
      console.log('===', item)
      res.status(200).send(item)
    })
    .catch((err) => {
      console.log(err)
      console.log('=== Error invoking DynamoDB GetItem!')
      res.status(500).send({ message: 'Error 500 - Server Error' })
    })
}

const POST = async (req: NextApiRequest, res: NextApiResponse) => {
  const { name } = req.query
  if (!name) res.status(400).send({ message: 'Error 400 - Bad Request' })

  const params = {
    TableName: 'GME-Project-2-Table',
    Key: { Name: { S: name } },
  }
  let request = dynamodb.getItem(params)
  await request
    .promise()
    .then((data) => {
      if (!data.Item) {
        console.log(`=== User "${name}" does not exist. Bad request!`)
        res.status(400).send({ message: 'Error 400 - Bad Request' })
        return
      }

      console.log(`=== User "${name}" successfully retrieved!`)
      console.log('=== DB Entry =', data.Item)
      const item = AWS.DynamoDB.Converter.unmarshall(data.Item)
      console.log('===', item)
      res.status(200).send(item)
    })
    .catch((err) => {
      console.log(err)
      console.log('=== Error invoking DynamoDB GetItem!')
      res.status(500).send({ message: 'Error 500 - Server Error' })
    })
}

const handler = async (req: NextApiRequest, res: NextApiResponse) => {
  if (req.method === 'GET') {
    return await GET(req, res)
  } else if (req.method === 'POST') {
    return await POST(req, res)
  }
}

export default handler
