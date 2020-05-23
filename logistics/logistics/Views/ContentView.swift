//
//  ContentView.swift
//  logistics
//
//  Created by Ivan on 16.05.2020.
//  Copyright © 2020 ITMO. All rights reserved.
//

import SwiftUI
import MapKit
import CoreData

struct ContentView: View {
    @Environment(\.managedObjectContext) var managedObjectContext
    @FetchRequest(fetchRequest: PorterDocument.allPorters()) var porters: FetchedResults<PorterDocument>
    
    @EnvironmentObject var appState: AppState
    
    @State var text = "The text"
    @State var textField = ""
    @State var showingDetail = false
    
    @State var locationManager = CLLocationManager()
    @State var showMapAlert = false
    
    var body: some View {
        VStack {
            TabView {
                PortersList()
                    .tabItem({
                        Image(systemName: "cloud")
                        Text("Admin Panel")
                    }).tag(0)
                
                PortersMap()
                    .tabItem({
                        Image(systemName: "play")
                        Text("User Menu")
                    }).tag(1)
            }
            .onAppear {
                let dbPorters = PorterDocument.toUIPorters(porters: self.porters)
                
                self.appState.porters = dbPorters
            }
        }
    }
    
    func goToDeviceSettings() {
        guard let url = URL.init(string: UIApplication.openSettingsURLString) else { return }
        UIApplication.shared.open(url, options: [:], completionHandler: nil)
    }
}

struct ContentView_Previews: PreviewProvider {
    static var previews: some View {
        let context = (UIApplication.shared.delegate as! AppDelegate).persistentContainer.viewContext
        
        return ContentView()
            .environmentObject(AppState())
            .environment(\.managedObjectContext, context)
    }
}

