using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncApp
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable GetCategories()
        {
            return ExecuteQuery("SELECT * FROM MatIndex");
        }

        public DataTable GetProducts()
        {
            var sql = @"
WITH CatHierarchy AS (
    -- Start with the product’s assigned category (level 0)
    SELECT 
       m.ID AS Mat_ID,
       mi.ID AS CatID,
       mi.NameA AS CatName,
       mi.MatIndex_ID AS ParentID,
       0 AS Level
    FROM FOG.dbo.Mat m
    JOIN FOG.dbo.MatIndex mi ON m.MatIndex_ID = mi.ID

    UNION ALL

    -- Climb up one level at a time
    SELECT
       ch.Mat_ID,
       p.ID AS CatID,
       p.NameA AS CatName,
       p.MatIndex_ID AS ParentID,
       ch.Level + 1 AS Level
    FROM CatHierarchy ch
    JOIN FOG.dbo.MatIndex p ON ch.ParentID = p.ID
),
MaxLevel AS (
    -- Determine the highest level reached (the root) for each product
    SELECT Mat_ID, MAX(Level) AS MaxLevel
    FROM CatHierarchy
    GROUP BY Mat_ID
)
SELECT 
    top 10
    m.ID AS guid,
    m.Code ,
    m.NameA ,
    m.NameE ,
    U1Price1,
    Spec,
    VAT,
    i.Image AS Mat_Image,


    -- The record at the maximum level is the root (Main Category)
    root.CatName AS MainCategory,
    -- If there is more than one level, then the record one level below the root is the Sub Category;
    -- if the product’s category is top-level (MaxLevel = 0), then SubCategory is NULL.
    CASE WHEN maxl.MaxLevel > 0 THEN sub.CatName ELSE NULL END AS SubCategory
FROM MaxLevel maxl
JOIN CatHierarchy root 
  ON maxl.Mat_ID = root.Mat_ID AND root.Level = maxl.MaxLevel
LEFT JOIN CatHierarchy sub 
  ON sub.Mat_ID = root.Mat_ID AND sub.Level = maxl.MaxLevel - 1
JOIN FOG.dbo.Mat m ON m.ID = maxl.Mat_ID
LEFT JOIN FOG.dbo.Images i ON m.Images_ID = i.ID AND i.Type = 'Mat'
where i.Image  is not null

";
            return ExecuteQuery(sql);
        }

        public DataTable GetOrders()
        {
            return ExecuteQuery("SELECT * FROM Orders");
        }

        private DataTable ExecuteQuery(string query)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }
    }
}
