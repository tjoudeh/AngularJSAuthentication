//Collection Setup
db.users.ensureIndex({ userId: 1 }, { unique: true, dropDups: true });

//userId = Stuart Shay 
db.users.find({ "userId": "116590040434310834456" });


db.users.find()





