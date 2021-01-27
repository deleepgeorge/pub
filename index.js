let indexes = 
{
	"collections" : [
		{
			"name" : "hangfire.mongo.jobGraph",
			"indexes" : [
				{
					"name" : "_id_",
					"key" : {
						"_id" : 1
					}
				},
				{
					"name" : "Key",
					"key" : {
						"Key" : 1
					},
					"unique" : true
				},
				{
					"name" : "StateName",
					"key" : {
						"StateName" : -1
					}
				},
				{
					"name" : "ExpireAt",
					"key" : {
						"ExpireAt" : -1
					}
				},
				{
					"name" : "_t",
					"key" : {
						"_t" : -1
					}
				},
				{
					"name" : "Queue",
					"key" : {
						"Queue" : -1
					}
				},
				{
					"name" : "FetchedAt",
					"key" : {
						"FetchedAt" : -1
					}
				},
				{
					"name" : "Value",
					"key" : {
						"Value" : -1
					}
				},
				{
					"name" : "Item",
					"key" : {
						"Item" : -1
					}
				}
			]
		},
		{
			"name" : "hangfire.mongo.locks",
			"indexes" : [
				{
					"name" : "_id_",
					"key" : {
						"_id" : 1
					}
				},
				{
					"name" : "ExpireAt",
					"key" : {
						"ExpireAt" : -1
					}
				},
				{
					"name" : "Resource",
					"key" : {
						"Resource" : -1
					},
					"unique" : true
				}
			]
		},
		{
			"name" : "hangfire.mongo.migrationLock",
			"indexes" : [
				{
					"name" : "_id_",
					"key" : {
						"_id" : 1
					}
				}
			]
		},
		{
			"name" : "hangfire.mongo.notifications",
			"indexes" : [
				{
					"name" : "_id_",
					"key" : {
						"_id" : 1
					}
				}
			]
		},
		{
			"name" : "hangfire.mongo.schema",
			"indexes" : [
				{
					"name" : "_id_",
					"key" : {
						"_id" : 1
					}
				}
			]
		},
		{
			"name" : "hangfire.mongo.server",
			"indexes" : [
				{
					"name" : "_id_",
					"key" : {
						"_id" : 1
					}
				},
				{
					"name" : "LastHeartbeat",
					"key" : {
						"LastHeartbeat" : -1
					}
				}
			]
		}
	]
}
;
