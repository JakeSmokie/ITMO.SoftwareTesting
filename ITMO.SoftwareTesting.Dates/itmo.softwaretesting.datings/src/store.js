import Vue from 'vue';
import Vuex from 'vuex';
import {favoriteEventsModule} from './store/favorite-events';
import {eventsModule} from './store/events';
import {kudagoModule} from './store/kudago';
import {groupsModule} from './store/groups';

Vue.use(Vuex);

export const store = new Vuex.Store({
	modules: {
		events: eventsModule,
		'events/favorites': favoriteEventsModule,
		kudago: kudagoModule,
		groups: groupsModule,
	},
});
