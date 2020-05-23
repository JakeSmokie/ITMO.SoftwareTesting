//
//  NewPorterLocationSelection.swift
//  logistics
//
//  Created by Ivan on 18.05.2020.
//  Copyright © 2020 ITMO. All rights reserved.
//

import SwiftUI
import MapKit

struct PorterLocationSelection: View {
    @Environment(\.presentationMode) var presentationMode;
    
    @Binding var address: String
    @Binding var coords: CLLocationCoordinate2D
    
    var body: some View {
        VStack(alignment: .trailing) {
            Button(action: { self.presentationMode.wrappedValue.dismiss() }) {
                Text("Close").padding(.horizontal)
            }
            
            ZStack {
                MapWithSelection(address: self.$address, coords: self.$coords)
                Circle()
                    .fill(Color.blue)
                    .opacity(0.3)
                    .frame(width: 16, height: 16)
                Text(self.address)
                    .multilineTextAlignment(.center)
                    .offset(y: -100)
                    .font(.title)
                    .padding()
                    .accessibility(identifier: "Map address")
            }
        }
        .padding(.vertical)
    }
}

struct NewPorterLocationSelection_Previews: PreviewProvider {
    static var previews: some View {
        PreviewWrapper()
    }
    
    struct PreviewWrapper: View {
        @State var address = ""
        @State var coords = CLLocationCoordinate2D()
        
        var body: some View {
            PorterLocationSelection(address: $address, coords: $coords)
        }
    }
}
