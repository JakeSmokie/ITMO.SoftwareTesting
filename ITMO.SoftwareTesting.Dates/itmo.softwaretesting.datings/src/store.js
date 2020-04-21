import Vue from 'vue';
import Vuex from 'vuex';
import {eventsModule} from './store/events';
import {kudagoModule} from './store/kudago';
import {groupsModule} from './store/groups';
import {datesModule} from './store/dates';

Vue.use(Vuex);

export const store = new Vuex.Store({
	state: {
		datesPageLoaded: false,
	},

	modules: {
		events: eventsModule,
		kudago: kudagoModule,
		groups: groupsModule,
		dates: datesModule,
	},

	mutations: {
		setDatesPageLoaded(state) {
			state.datesPageLoaded = true;
		},
	},

	getters: {
		availableGroups: (state, getters) =>
			(getters['groups/myGroups'] || [])
				.filter(x => !(state.dates.dates[x.id] || []).includes(state.events.selectedEvent)),
	},

	actions: {
		async loadDatesPage({commit, dispatch}) {
			await dispatch('dates/loadDates');
			await dispatch('groups/listGroups');
			await dispatch('loadEventDetailsForDates');

			commit('setDatesPageLoaded');
		},

		async loadEventDetailsForDates({commit, dispatch, state}) {
			for (let [, eventIds] of Object.entries(state.dates.dates)) {
				for (const eventId of eventIds) {
					await dispatch('events/loadEventDetails', eventId);
				}
			}
		},
	},
});
