//Collection Setup
db.refreshTokens.ensureIndex({ subject: 1, clientId:1 }, { unique: true, dropDups: true });

db.refreshTokens.find()





