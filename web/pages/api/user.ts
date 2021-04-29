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

const handler = async (req: NextApiRequest, res: NextApiResponse) => {
  if (req.method === 'GET') {
    console.log('\nGET /user')
    const { name } = req.query
    if (!name) {
      return res.status(400).send({ message: 'Error 400 - Bad Request' })
    }

    const params = {
      TableName: 'GME-Project-2-Table',
      Key: { Name: { S: name } },
    }
    let request = dynamodb.getItem(params)
    try {
      const data = await request.promise()

      if (!data.Item) {
        console.log(`=== User "${name}" does not exist. Bad request!`)
        return res.status(400).send({ message: 'Error 400 - Bad Request' })
      }

      console.log(`=== User "${name}" successfully retrieved!`)
      console.log('=== DB Entry =', data.Item)
      const item = AWS.DynamoDB.Converter.unmarshall(data.Item)
      console.log('===', item)
      return res.status(200).send(item)
    } catch (e) {
      console.log(e)
      console.log('=== Error invoking DynamoDB GetItem!')
      return res.status(500).send({ message: 'Error 500 - Server Error' })
    }
  } else if (req.method === 'POST') {
    const { name } = req.query
    if (!name) res.status(400).send({ message: 'Error 400 - Bad Request' })

    const params = {
      TableName: 'GME-Project-2-Table',
      Key: { Name: { S: name } },
    }
    let request = dynamodb.getItem(params)
    try {
      const data = await request.promise()

      if (!data.Item) {
        console.log(`=== User "${name}" does not exist. Bad request!`)
        return res.status(400).send({ message: 'Error 400 - Bad Request' })
      }

      console.log(`=== User "${name}" successfully retrieved!`)
      console.log('=== DB Entry =', data.Item)
      const item = AWS.DynamoDB.Converter.unmarshall(data.Item)
      console.log('===', item)
      return res.status(200).send(item)
    } catch (e) {
      console.log(e)
      console.log('=== Error invoking DynamoDB GetItem!')
      return res.status(500).send({ message: 'Error 500 - Server Error' })
    }
  }
}

export default handler
