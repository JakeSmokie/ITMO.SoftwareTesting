//
//  PortersView.swift
//  logistics
//
//  Created by Ivan on 19.05.2020.
//  Copyright © 2020 ITMO. All rights reserved.
//

import SwiftUI

struct PortersView: View {
    @EnvironmentObject var appState: AppState

    var body: some View {
        VStack {
            PortersList()
        }
    }
}

struct PortersView_Previews: PreviewProvider {
    static var previews: some View {
        PortersView()
            .environmentObject(AppState())
    }
}
