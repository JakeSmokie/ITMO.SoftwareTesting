//
//  PortersMap.swift
//  logistics
//
//  Created by Ivan on 19.05.2020.
//  Copyright © 2020 ITMO. All rights reserved.
//

import SwiftUI
import MapKit
import Foundation
import Combine

struct PortersMap: UIViewRepresentable {
    @EnvironmentObject var appState: AppState

    func makeUIView(context: Context) -> MKMapView {
        let mapView = MKMapView(frame: .zero)
        return mapView
    }
    
    func updateUIView(_ mapView: MKMapView, context: Context) {
        appState.porters.forEach { porter in
            let annotation = MKPointAnnotation()
            annotation.coordinate = CLLocationCoordinate2D(latitude: porter.latitude, longitude: porter.longitude)
            annotation.title = porter.name
            annotation.subtitle = porter.contact
            annotation.accessibilityLabel = "mapMarker"
            
            mapView.addAnnotation(annotation)
        }
    }
}

struct PortersMap_Previews: PreviewProvider {
    static var previews: some View {
        PortersMap()
            .environmentObject(AppState())
    }
}
