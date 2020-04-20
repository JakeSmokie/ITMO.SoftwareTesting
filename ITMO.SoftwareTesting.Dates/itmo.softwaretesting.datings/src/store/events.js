import {eventDetails, listEvents, placeDetails} from '../services/kudago.service';

export const eventsModule = {
	namespaced: true,
	state: {
		events: [],
		selectedEvent: null,
		selectedEventDetails: null,
	},

	mutations: {
		selectEvent(state, eventId) {
			state.selectedEvent = eventId;
		},

		setEventDetails(state, details) {
			state.selectedEventDetails = details;
		},

		loadEvents(state, events) {
			state.events = events;
		},
	},

	actions: {
		async selectEvent({commit}, eventId) {
			commit('selectEvent', null);
			commit('setEventDetails', null);

			const details = await eventDetails(eventId);
			const place = details.place || {};

			if (place.id) {
				details.place = await placeDetails(place.id);
			}

			commit('selectEvent', eventId);
			commit('setEventDetails', details);
		},

		async loadEvents({commit, dispatch}, [category, location] = []) {
			const events = await listEvents(location, category);

			commit('loadEvents', events);
			await dispatch('selectEvent', events[0].id);
		},
	},
};
