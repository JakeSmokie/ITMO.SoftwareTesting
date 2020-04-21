import {eventDetails, listEvents, placeDetails} from '../services/kudago.service';
import {addEventInFavorites, favoriteEvents, removeEventFromFavorites} from '../services/favorite-events.service';

export const eventsModule = {
	namespaced: true,
	state: {
		events: [],
		eventsDetails: [],
		selectedEvent: null,
		favoriteEvents: [],
		favoriteEventsLoaded: false,
	},

	getters: {
		selectedEventDetails: state => state.eventsDetails.find(x => x.id === state.selectedEvent),
		favoriteEventsDetails: state => (state.eventsDetails || []).filter(x => state.favoriteEvents.includes(x.id)),
	},

	mutations: {
		selectEvent(state, eventId) {
			state.selectedEvent = eventId;
		},

		addEventDetails(state, details) {
			state.eventsDetails.push(details);
		},

		loadEvents(state, events) {
			state.events = events;
		},

		addFavoriteEvent(state, event) {
			state.favoriteEvents.push(event);
		},

		removeFavoriteEvent(state, event) {
			state.favoriteEvents = state.favoriteEvents.filter(x => x !== event);
		},

		loadFavoriteEvents(state, events) {
			state.favoriteEvents = events;
		},

		setFavoriteEventsLoaded(state) {
			state.favoriteEventsLoaded = true;
		},
	},

	actions: {
		async loadEventDetails({commit, state}, eventId) {
			if (state.eventsDetails.some(x => x.id === eventId)) {
				return;
			}

			const details = await eventDetails(eventId);
			const place = details.place || {};

			if (place.id) {
				details.place = await placeDetails(place.id);
			}

			commit('addEventDetails', details);
		},

		async selectEvent({commit, dispatch}, eventId) {
			commit('selectEvent', null);
			await dispatch('loadEventDetails', eventId);
			commit('selectEvent', eventId);
		},

		async loadEvents({commit, dispatch}, [category, location] = []) {
			const events = await listEvents(location, category);

			commit('loadEvents', events);
			await dispatch('selectEvent', events[0].id);
		},

		async loadFavoriteEvents({commit, dispatch}) {
			const favorites = await favoriteEvents();
			commit('loadFavoriteEvents', favorites);

			for (const id of favorites) {
				await dispatch('loadEventDetails', id);
			}

			commit('setFavoriteEventsLoaded');
		},

		async addFavoriteEvent({commit, dispatch}, event) {
			await addEventInFavorites(event);
			await dispatch('loadEventDetails', event);

			commit('addFavoriteEvent', event);
		},

		async removeFavoriteEvent({commit}, event) {
			await removeEventFromFavorites(event);
			commit('removeFavoriteEvent', event);
		},
	},
};
