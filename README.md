# SqlQueryBuilder
 This project has the propouse of transform a json content into a SQL query

## Load Json file
give the path of a valid json file to imput the content to the database

## Seed sample Json
Seeds automaticaly the sample json on the database to test 

## Process all database content
Get all data from the database, an processesses the content into the query output

## Delete all database content
Cleans the database data for a fresh start test

## Export all Json files stored in the database
Export all Json stored in the database with the output query generated in the given path


Sample Json extruture
`
{
   "table": "orders",
   "top": "0",
   "columns": [],
   "where": [
      {
         "operator": "Equals",
         "fieldName": "orders.store_id",
         "fieldValue": "1"
      }
   ],
   "join": []
}
`

** The Sql query transform engine was based on the following article https://www.codeproject.com/Articles/13419/SelectQueryBuilder-Building-complex-and-flexible-S 