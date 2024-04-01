const fs = require('fs');
const path = require('path');

function loadTestData(fileName) {
    const filePath = path.join(__dirname, fileName);
    const rawData = fs.readFileSync(filePath);
    const jsonData = JSON.parse(rawData);
    return jsonData.cases.map(testCase => ({
        description: testCase.description, 
        number: testCase.input.number, 
        expected: testCase.expected
    }));
}

module.exports = loadTestData;