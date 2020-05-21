//
//  PortersListItem.swift
//  logistics
//
//  Created by Ivan on 19.05.2020.
//  Copyright © 2020 ITMO. All rights reserved.
//

import SwiftUI
import CoreLocation

struct PortersListItem: View {
    var porter: Porter
    
    var body: some View {
        VStack(alignment: .leading) {
            Text(porter.name).bold().padding(.vertical).font(.title)
            
            Text("Contact").font(.headline)
            Text(porter.contact).padding(.bottom)
            
            Text("Address").font(.headline)
            Text(porter.address).padding(.bottom)
        }
    }
}

struct PortersListItem_Previews: PreviewProvider {
    static var previews: some View {
        PortersListItem(porter: Porter(address: "Address", contact: "1234", name: "Name"))
    }
}
