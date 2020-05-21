//
//  Map.swift
//  logistics
//
//  Created by Ivan on 16.05.2020.
//  Copyright © 2020 ITMO. All rights reserved.
//

import SwiftUI
import MapKit
import RxSwift

struct MapWithSelection: UIViewRepresentable {
    @Binding var address: String
    @Binding var coords: CLLocationCoordinate2D

    init (
        address: Binding<String>,
        coords: Binding<CLLocationCoordinate2D>
    ) {
        _address = address
        _coords = coords
    }
    
    func makeUIView(context: Context) -> MKMapView {
        let mapView = MKMapView(frame: .zero)
        mapView.delegate = context.coordinator
        
        let span = MKCoordinateSpan(latitudeDelta: 1.0, longitudeDelta: 1.0)
        let region = MKCoordinateRegion(center: self.coords, span: span)
        mapView.setRegion(region, animated: true)
        
        return mapView
    }
    
    func updateUIView(_ mapView: MKMapView, context: Context) {
    }
    
    func makeCoordinator() -> Coordinator {
        Coordinator(self)
    }
    
    class Coordinator: NSObject, MKMapViewDelegate {
        var parent: MapWithSelection
        var centerCoordinateSubject = PublishSubject<CLLocationCoordinate2D>()
        var geocoder = CLGeocoder()
        
        init(_ parent: MapWithSelection) {
            self.parent = parent
            let geocoder = self.geocoder

            _ = centerCoordinateSubject
                .asObservable()
                .debounce(RxTimeInterval.milliseconds(500), scheduler: MainScheduler.instance)
                .subscribe(onNext: { value in
                    geocoder.reverseGeocodeLocation(CLLocation(latitude: value.latitude, longitude: value.longitude)) { optionalPlacemark, _ in
                        guard let placemark = optionalPlacemark else {
                            return
                        }

                        parent.address = (placemark[0].subAdministrativeArea ?? "") + ", " + (placemark[0].name ?? "")
                        parent.coords = value
                    }
                })
        }
        
        func mapViewDidChangeVisibleRegion(_ mapView: MKMapView) {
            self.centerCoordinateSubject.onNext(mapView.centerCoordinate)
        }
    }
}

struct MapWithSelection_Previews: PreviewProvider {
    static var previews: some View {
        MapWithSelection(address: .constant(""), coords: .constant(CLLocationCoordinate2D(latitude: 60, longitude: 30)))
    }
}
