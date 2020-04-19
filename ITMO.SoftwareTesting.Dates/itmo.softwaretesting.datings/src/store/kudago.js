import {eventCategories, locations} from '../services/kudago.service';

export const kudagoModule = {
	namespaced: true,
	state: {
		eventCategories: [],
		locations: [],
	},

	mutations: {
		setEventCategories(state, payload) {
			state.eventCategories = payload;
		},

		setLocations(state, payload) {
			state.locations = payload;
		},
	},

	actions: {
		async loadEventCategories({commit}) {
			commit('setEventCategories', await eventCategories());
		},

		async loadLocations({commit}) {
			commit('setLocations', await locations());
		},
	},
};
