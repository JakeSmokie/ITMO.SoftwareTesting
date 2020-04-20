import {addEventInFavorites, favoriteEvents, removeEventFromFavorites} from '../services/favorite-events.service';
import {eventDetails} from '../services/kudago.service';

export const favoriteEventsModule = {
	namespaced: true,
	state: {
		favoriteEvents: [],
		favoriteEventsDetails: [],
		selectedEvent: null,
	},

	getters: {
		selectedEventDetails: state => state.favoriteEventsDetails.find(x => x.id === state.selectedEvent),
	},

	mutations: {
		addFavoriteEvent(state, [event, details]) {
			state.favoriteEvents.push(event);
			state.favoriteEventsDetails.push(details);
		},

		removeFavoriteEvent(state, event) {
			state.favoriteEvents = state.favoriteEvents.filter(x => x !== event);
			state.favoriteEventsDetails = state.favoriteEventsDetails.filter(x => x.id !== event);
			state.selectedEvent = state.favoriteEvents[0];
		},

		loadFavoriteEvents(state, events) {
			state.favoriteEvents = events;
		},

		setFavoriteEventsDetails(state, details) {
			state.favoriteEventsDetails = details;
		},

		setSelectedEvent(state, event) {
			state.selectedEvent = event;
		},
	},

	actions: {
		async loadFavoriteEventsDetails({commit, state, dispatch}) {
			commit('setFavoriteEventsDetails', []);
			await dispatch('loadFavoriteEvents');

			const details = [];

			for (const id of state.favoriteEvents) {
				details.push(await eventDetails(id));
			}

			commit('setFavoriteEventsDetails', details);
			commit('setSelectedEvent', state.favoriteEvents[0]);
		},

		async loadFavoriteEvents({commit}) {
			commit('loadFavoriteEvents', await favoriteEvents());
		},

		async addFavoriteEvent({commit}, event) {
			await addEventInFavorites(event);

			commit('addFavoriteEvent', [event, await eventDetails(event)]);
		},

		async removeFavoriteEvent({commit}, event) {
			await removeEventFromFavorites(event);
			commit('removeFavoriteEvent', event);
		},
	},
};
