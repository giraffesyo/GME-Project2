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

const dynamodb = new AWS.DynamoDB()

const handler = async (req: NextApiRequest, res: NextApiResponse) => {
  if (req.method === 'GET') {
    console.log('\nGET /user')
    const { name } = req.query
    if (!name) {
      return res.status(400).json({ message: 'Error 400 - Bad Request' })
    }

    const params = {
      TableName: 'GME-Project-2-Table',
      Key: { Name: { S: name } },
    }
    try {
      const data = await dynamodb.getItem(params).promise()

      if (!data.Item) {
        console.log(`=== User "${name}" does not exist. Bad request!`)
        return res.status(400).json({ message: 'Error 400 - Bad Request' })
      }

      console.log(`=== User "${name}" successfully retrieved!`)
      console.log('=== DB Entry =', data.Item)
      const item = AWS.DynamoDB.Converter.unmarshall(data.Item)
      console.log('===', item)
      return res.status(200).json(item)
    } catch (e) {
      const message = `=== Error invoking DynamoDB GetItem!`
      console.error(e)
      console.error(message)
      return res.status(500).json({ message })
    }
  } else if (req.method === 'POST') {
    console.log('\nPOST /user')
    console.log('=== Body: ', req.body)
    const { name, score } = req.body
    if (!(name && typeof score == 'number')) {
      const message = `Returning 400 because name or score invalid, name: ${name}, score: ${score}`
      console.error(message)
      return res.status(400).json({ message })
    }

    console.log('Ok.')

    const reqParams = {
      TableName: 'GME-Project-2-Table',
      Key: { Name: { S: name } },
    }

    try {
      const data = await dynamodb.getItem(reqParams).promise()

      if (!data.Item) throw 'User does not exist'
      console.log(`=== User "${name}" exists. Updating user...`)
      console.log('=== Current DB Entry: ', data.Item)

      const setParams = {
        TableName: 'GME-Project-2-Table',
        Key: { Name: { S: name } },
        ExpressionAttributeValues: { ':score': { N: score } },
        UpdateExpression: 'SET Score = :score',
      }

      try {
        await dynamodb.updateItem(setParams).promise()
        console.log(`=== User "${name}" has been successfully updated!`)
        return res.status(200).json({
          message: `User "${name}" has been successfully updated!`,
        })
      } catch (err) {
        console.error(err)
        console.error('=== Error invoking DynamoDB UpdateItem!')
        return res.status(500).json({ message: 'Error 500 - Server Error' })
      }
    } catch (err) {
      console.log(`=== User "${name}" does not exist. Creating user...`)

      const item = AWS.DynamoDB.Converter.marshall({
        Name: name,
        Score: score,
      })
      const setParams = { TableName: 'GME-Project-2-Table', Item: item }
      try {
        await dynamodb.putItem(setParams).promise()
        console.log(`=== User "${name}" has been successfully updated!`)
        return res.status(200).json({
          message: `User "${name}" has been successfully created!`,
        })
      } catch (err) {
        console.error(err)
        console.error('=== Error invoking DynamoDB PutItem!')
        return res.status(500).json({ message: 'Error 500 - Server Error' })
      }
    }
  }
}

export default handler
