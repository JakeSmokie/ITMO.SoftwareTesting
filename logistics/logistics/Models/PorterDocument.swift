//
//  PorterDocumentExtensions.swift
//  logistics
//
//  Created by Ivan on 21.05.2020.
//  Copyright © 2020 ITMO. All rights reserved.
//

import Foundation
import CoreData
import SwiftUI

extension PorterDocument {
    static func allPorters() -> NSFetchRequest<PorterDocument> {
        let request: NSFetchRequest<PorterDocument> = PorterDocument.fetchRequest()
        
        request.sortDescriptors = [
            NSSortDescriptor(key: "name", ascending: true)
        ]
        
        return request
    }
    
    static func toUIPorters(porters: FetchedResults<PorterDocument>) -> [Porter] {
        porters.map { x in
            Porter(id: x.id!, address: x.address!, contact: x.contact!, name: x.name!, latitude: x.latitude, longitude: x.longitude)
        }
    }
    
    static func createPorter(porter: Porter, context: NSManagedObjectContext) {
        let doc = PorterDocument(context: context)
        
        doc.id = porter.id
        doc.address = porter.address
        doc.contact = porter.contact
        doc.latitude = porter.latitude
        doc.longitude = porter.longitude
        doc.name = porter.name
        
        do {
            try context.save()
        } catch {
            print(error)
        }
    }
    
    static func savePorter(porter: Porter, context: NSManagedObjectContext) {
        let request: NSFetchRequest<PorterDocument> = PorterDocument.fetchRequest()
        request.predicate = NSPredicate(format: "id == %@", porter.id.uuidString)
        
        do {
            let doc = try context.fetch(request).first!
            
            doc.id = porter.id
            doc.address = porter.address
            doc.contact = porter.contact
            doc.latitude = porter.latitude
            doc.longitude = porter.longitude
            doc.name = porter.name
            
            try context.save()
        } catch {
            print(error)
        }
    }
    
    static func deletePorter(porter: Porter, context: NSManagedObjectContext) {
        let request: NSFetchRequest<PorterDocument> = PorterDocument.fetchRequest()
        request.predicate = NSPredicate(format: "id == %@", porter.id.uuidString)
        
        do {
            let doc = try context.fetch(request).first!
            
            context.delete(doc)
            try context.save()
        } catch {
            print(error)
        }
    }
}
