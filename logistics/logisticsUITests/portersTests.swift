import XCTest

class logisticsUITests: XCTestCase {
    
    override func setUpWithError() throws {
        continueAfterFailure = false
    }
    
    override func tearDownWithError() throws {
    }
    
    func testLaunch() throws {
        let app = XCUIApplication()
        app.launch()
    }
    
    func testShowingPlaceholderTextIfNoPortersFound() throws {
        let app = XCUIApplication()
        app.launch()
        app.buttons["Clear"].tap()
        XCTAssertTrue(app.staticTexts["No porters text"].exists)
    }
    
    func testPorterAdding() throws {
        let name = "Durov \(Date().timeIntervalSince1970)"
        let contact = "tg: @durov"
        let address = "Saint Petersburg, Finskiy Zaliv"
        
        let app = XCUIApplication()
        app.launch()

        app.buttons["Clear"].tap()
        app.buttons["Add"].tap()
        app.textFields["Name"].tapAndType(name + "\n")
        app.textFields["Contact"].tapAndType(contact + "\n")

        app.buttons["Choose location"].tap()
        XCTAssertTrue(app.staticTexts["Map address"].waitForExistence(timeout: 1))

        app.buttons["Close"].tap()
        app.buttons["Save"].tap()

        app.terminate()
        app.launch()

        XCTAssertTrue(app.buttons[porterButton(name, contact, address)].exists)
        app.buttons["Clear"].tap()
    }
    
    func testPorterEditing() throws {
        let oldName = "RandomName"
        let newName = "Durov \(Date().timeIntervalSince1970)"
        let oldContact = "vk"
        let newContact = "tg: @durov"
        let address = "Saint Petersburg, Finskiy Zaliv"
        
        let app = XCUIApplication()
        app.launch()

        app.buttons["Clear"].tap()
        app.buttons["Add"].tap()
        app.textFields["Name"].tapAndType(oldName + "\n")
        app.textFields["Contact"].tapAndType(oldContact + "\n")

        app.buttons["Choose location"].tap()
        XCTAssertTrue(app.staticTexts["Map address"].waitForExistence(timeout: 1))

        app.buttons["Close"].tap()
        app.buttons["Save"].tap()
        
        app.buttons[porterButton(oldName, oldContact, address)].tap()
        app.textFields["Name"].tapAndType(newName + "\n")
        app.textFields["Contact"].tapAndType(newContact + "\n")
        app.buttons["Save"].tap()

        app.terminate()
        app.launch()

        XCTAssertTrue(app.buttons[porterButton(newName, newContact, address)].exists)
        app.buttons["Clear"].tap()
    }
    
    func testPorterDeleting() throws {
        let oldName = "RandomName"
        let oldContact = "vk"
        let address = "Saint Petersburg, Finskiy Zaliv"
        
        let app = XCUIApplication()
        app.launch()

        app.buttons["Clear"].tap()
        app.buttons["Add"].tap()
        app.textFields["Name"].tapAndType(oldName + "\n")
        app.textFields["Contact"].tapAndType(oldContact + "\n")

        app.buttons["Choose location"].tap()
        XCTAssertTrue(app.staticTexts["Map address"].waitForExistence(timeout: 1))

        app.buttons["Close"].tap()
        app.buttons["Save"].tap()
        
        app.buttons[porterButton(oldName, oldContact, address)].swipeLeft()
        app.buttons["Delete"].tap()

        app.terminate()
        app.launch()

        XCTAssertFalse(app.buttons[porterButton(oldName, oldContact, address)].exists)
        app.buttons["Clear"].tap()
    }
    
    func testPorterMap() throws {
        let oldName = "RandomName"
        let oldContact = "vk"

        let app = XCUIApplication()
        app.launch()

        app.buttons["Clear"].tap()
        app.buttons["Add"].tap()
        app.textFields["Name"].tapAndType(oldName + "\n")
        app.textFields["Contact"].tapAndType(oldContact + "\n")

        app.buttons["Choose location"].tap()
        XCTAssertTrue(app.staticTexts["Map address"].waitForExistence(timeout: 1))

        app.buttons["Close"].tap()
        app.buttons["Save"].tap()
        
        app.buttons["User Menu"].tap()
        
        app.maps.firstMatch.swipeRight()
        XCTAssertTrue(app.maps.containing(NSPredicate(format: "label == %@", oldName)).firstMatch.waitForExistence(timeout: 3))

        app.buttons["Admin Panel"].tap()
        app.buttons["Clear"].tap()
    }

    
    func testIfSaveButtonDisabledOnEmptyNameAndContactOrMap() throws {
        let oldName = "RandomName"
        let oldContact = "vk"

        let app = XCUIApplication()
        app.launch()

        app.buttons["Clear"].tap()
        app.buttons["Add"].tap()

        app.textFields["Name"].tapAndType(oldName + "\n")
        app.textFields["Contact"].tapAndType(oldContact + "\n")

        let first = app.buttons["Save"].isEnabled
        
        app.buttons["Choose location"].tap()
        XCTAssertTrue(app.staticTexts["Map address"].waitForExistence(timeout: 1))
        app.buttons["Close"].tap()

        app.textFields["Name"].tapAndType("\n")
        let second = app.buttons["Save"].isEnabled
        
        app.textFields["Name"].tapAndType(oldName + "\n")
        app.textFields["Contact"].tapAndType("\n")
        
        let third = app.buttons["Save"].isEnabled
        
        XCTAssertFalse(first)
        XCTAssertFalse(second)
        XCTAssertFalse(third)
    }

    
    func porterButton(_ name: String, _ contact: String, _ address: String) -> String {
        "\(name)\nContact\n\(contact)\nAddress\n\(address)"
    }
}
