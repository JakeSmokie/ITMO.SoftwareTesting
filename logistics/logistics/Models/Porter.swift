//
//  Porter.swift
//  logistics
//
//  Created by Ivan on 18.05.2020.
//  Copyright © 2020 ITMO. All rights reserved.
//

import Foundation
import CoreLocation

struct Porter : Identifiable {
    var id: UUID = UUID()

    var address = ""
    var contact = ""
    var name = ""
    var latitude = 60.0
    var longitude = 30.0
}
