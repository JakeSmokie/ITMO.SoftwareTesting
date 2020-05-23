//
//  NewPorter.swift
//  logistics
//
//  Created by Ivan on 18.05.2020.
//  Copyright © 2020 ITMO. All rights reserved.
//

import SwiftUI
import MapKit

struct PorterEditForm: View {
    @Environment(\.presentationMode) var presentationMode;
    @State private var showingMap = false

    private var id = UUID();
    @State var name = ""
    @State var phone = ""
    @State var coords = CLLocationCoordinate2D(latitude: 59.9574, longitude: 30.3083)
    @State var address = ""
    
    var onSaved: (Porter) -> () = { _ in };
    
    init () {}
    init (porter: Porter, onSaved: @escaping (Porter) -> ()) {
        id = porter.id

        _name = .init(initialValue: porter.name)
        _phone = .init(initialValue: porter.contact)
        _coords = .init(initialValue: CLLocationCoordinate2D(latitude: porter.latitude, longitude: porter.longitude))
        _address = .init(initialValue: porter.address)

        self.onSaved = onSaved
    }

    var body: some View {
        VStack(alignment: .trailing) {
            Form {
                Section {
                    TextField("Name", text: $name)
                    TextField("Contact", text: $phone)
                }
                
                Section {
                    Button(action: toggleMap) {
                        Text("Choose location")
                    }
                    .sheet(isPresented: $showingMap) {
                        PorterLocationSelection(address: self.$address, coords: self.$coords)
                            .edgesIgnoringSafeArea(.all)
                    }.edgesIgnoringSafeArea(.all)

                    if self.address != "" {
                        Text(self.address).accessibility(identifier: "Address")
                        Text("\(self.coords.latitude) \(self.coords.longitude)")
                    }
                }
                
                Section {
                    Button(action: savePorter) {
                        Text("Save")
                    }
                    .disabled(isFormInvalid())
                }
            }
        }
    }
    
    func isFormInvalid() -> Bool {
        return self.name.count == 0 || self.phone.count == 0 || self.address == ""
    }
    
    func toggleMap() -> Void {
        self.showingMap.toggle()
    }
    
    func savePorter() -> Void {
        if isFormInvalid() { return }
        
        let porter = Porter(id: id, address: address, contact: phone, name: name, latitude: coords.latitude, longitude: coords.longitude)
        onSaved(porter)

        presentationMode.wrappedValue.dismiss()
    }
}

struct NewPorter_Previews: PreviewProvider {
    static var previews: some View {
        PorterEditForm()
            .environmentObject(AppState())
    }
}
