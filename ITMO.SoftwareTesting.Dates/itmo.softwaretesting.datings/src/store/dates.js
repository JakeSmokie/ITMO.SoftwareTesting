import Vue from 'vue';
import {createDate, listDates} from '../services/dates.service';

const groupBy = (xs, selector) =>
	xs.reduce((rv, x) => {
		(rv[selector(x)] = rv[selector(x)] || []).push(x);
		return rv;
	}, {});

export const datesModule = {
	namespaced: true,
	state: {
		dates: {},
	},

	mutations: {
		loadDates(state, newDates) {
			const groups = Object.entries(groupBy(newDates, x => x.groupId))
				.map(([group, dates]) => [+group, dates.map(x => x.eventId)]);

			for (const [group, dates] of groups) {
				Vue.set(state.dates, group, [...new Set([
					...dates,
					...(state.dates[group] || []),
				])]);
			}
		},

		createDate(state, date) {
			Vue.set(state.dates, date.groupId, [...new Set([
				date.eventId,
				...(state.dates[date.groupId] || [])]),
			]);
		},
	},

	actions: {
		async loadDates({commit}, groupId) {
			commit('loadDates', await listDates(groupId));
		},

		async createDate({commit}, [groupId, eventId]) {
			await createDate(groupId, eventId);
			commit('createDate', {groupId, eventId});
		},
	},
};
