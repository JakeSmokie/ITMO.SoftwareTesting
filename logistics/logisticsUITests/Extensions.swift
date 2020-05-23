//
//  Extensions.swift
//  logisticsUITests
//
//  Created by Ivan on 22.05.2020.
//  Copyright © 2020 ITMO. All rights reserved.
//

import XCTest

extension XCUIElement {
    func tapAndType(_ text: String) {
        guard let stringValue = self.value as? String else {
            XCTFail("Tried to clear and enter text into a non string value")
            return
        }
        
        self.tap()
        self.typeText(String(repeating: XCUIKeyboardKey.delete.rawValue, count: stringValue.count))
        self.typeText(text)
    }
}


