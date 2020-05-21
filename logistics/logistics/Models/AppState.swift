//
//  AppState.swift
//  logistics
//
//  Created by Ivan on 19.05.2020.
//  Copyright © 2020 ITMO. All rights reserved.
//

import Foundation
import SwiftUI
import MapKit

final class AppState : ObservableObject {
    @Published var porters: [Porter] = []
}
