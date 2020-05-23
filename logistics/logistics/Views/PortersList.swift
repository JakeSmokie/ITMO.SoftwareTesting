//
//  PortersList.swift
//  logistics
//
//  Created by Ivan on 19.05.2020.
//  Copyright © 2020 ITMO. All rights reserved.
//

import SwiftUI
import CoreData

struct PortersList: View {
    @Environment(\.managedObjectContext) var managedObjectContext
    @FetchRequest(entity: PorterDocument.entity(), sortDescriptors: []) var entityNames: FetchedResults<PorterDocument>
    @EnvironmentObject var appState: AppState
    
    @State var modalShown = false
    
    var body: some View {
        NavigationView  {
            VStack {
                if appState.porters.count > 0 {
                    List {
                        ForEach(appState.porters) { porter in
                            NavigationLink(
                                destination: PorterEditForm(porter: porter, onSaved: self.updatePorter)
                                    .navigationBarTitle("\(porter.name)", displayMode: .inline)
                            ) {
                                PortersListItem(porter: porter)
                            }
                        }
                        .onDelete(perform: deletePorter)
                    }
                    .accessibility(identifier: "Porters List")
                } else {
                    Spacer()
                    Text("No porters were found.\nAdd some")
                        .multilineTextAlignment(.center)
                        .padding(.bottom, 50)
                        .font(.title)
                        .accessibility(identifier: "No porters text")
                    Spacer()
                }
            }
            .navigationBarItems(
                leading: Button(action: {
                    for entityName in self.entityNames { self.managedObjectContext.delete(entityName) }
                    try? self.managedObjectContext.save()
                    
                    self.appState.porters = []
                }) {
                    Text("Clear").foregroundColor(.red)
                },
                trailing: HStack {
                    NavigationLink(
                        destination: PorterEditForm(porter: Porter(), onSaved: self.createPorter)
                            .navigationBarTitle("New porter", displayMode: .inline)
                            .environmentObject(self.appState)
                    ) {
                        Text("Add")
                    }
                    //                    Button(action: {self.modalShown.toggle()}) {
                    //                        Text("Add")
                    //                    }
                    //                    .sheet(isPresented: $modalShown) {
                    //                        PorterEditForm(porter: Porter(), onSaved: self.createPorter)
                    //                            .environmentObject(self.appState)
                    //                    }
                }
            )
            .navigationBarTitle("Porters", displayMode: .inline)
        }
    }
    
    func updatePorter(porter: Porter) {
        PorterDocument.savePorter(porter: porter, context: self.managedObjectContext)
        
        let index = appState.porters.firstIndex(where: { $0.id == porter.id })!
        appState.porters[index] = porter
    }
    
    func createPorter(porter: Porter) {
        PorterDocument.createPorter(porter: porter, context: self.managedObjectContext)
        
        appState.porters.append(porter)
        modalShown.toggle()
    }
    
    func deletePorter(offsets: IndexSet) {
        guard let index = Array(offsets).first else { return }
        
        let porter = self.appState.porters[index]
        PorterDocument.deletePorter(porter: porter, context: self.managedObjectContext)
        
        self.appState.porters.remove(at: index)
    }
}

struct PortersList_Previews: PreviewProvider {
    static var previews: some View {
        let context = (UIApplication.shared.delegate as! AppDelegate).persistentContainer.viewContext
        
        return PortersList()
            .environmentObject(AppState())
            .environment(\.managedObjectContext, context)
    }
}
