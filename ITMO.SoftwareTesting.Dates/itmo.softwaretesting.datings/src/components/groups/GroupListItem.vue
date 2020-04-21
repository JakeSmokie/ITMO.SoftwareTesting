<template>
	<div>
		<b-list-group-item action v-on:click="switchSpoiler">
			<b-row class="text-left align-items-center">
				<b-col cols="3">
					📖 Название: "{{ group.name }}"
				</b-col>
				<b-col>
					🧭 Назначение: "{{ group.purpose }}"
				</b-col>
				<b-col cols="auto" v-if="group.owner">
					<b-button size="sm" variant="outline-danger" v-on:click="removeGroup">Удалить</b-button>
				</b-col>
			</b-row>
		</b-list-group-item>
		<b-list-group-item v-if="shown" class="text-left">
			<template v-if="group.owner">
				<div>
					<b-form-group label="📖 Название" label-cols="2">
						<b-input v-model="name"/>
					</b-form-group>
					<b-form-group label="🧭 Назначение" label-cols="2">
						<b-input v-model="purpose"/>
					</b-form-group>

					<div class="d-flex justify-content-end">
						<b-button
							v-on:click="saveGroup"
							:disabled="name.trim() === '' || purpose.trim() === ''"
							variant="outline-success"
						>
							💾 Сохранить изменения
						</b-button>
					</div>
				</div>

				<hr>
			</template>

			<b-row>
				<b-col>
					<p>Члены группы:</p>
					<ul>
						<li v-for="user in group.members" :key="user.id">
							{{ user.nickname }}
							<b-button
								size="sm" variant="outline-danger" class="py-0 ml-2"
								v-if="group.owner && !user.me"
								v-on:click="deletePersonFromGroup({group: group.id, user: user.id})"
							>
								&cross;
							</b-button>
						</li>
					</ul>

					<template v-if="group.invites && group.invites.length > 0">
						<p>Приглашенные в группу:</p>
						<ul>
							<li v-for="user in group.invites" :key="user.id">{{ user.nickname }}</li>
						</ul>
					</template>
				</b-col>
				<b-col class="d-flex flex-column justify-content-start align-items-end" v-if="group.owner">
					<b-form-input
						:list="$id('users-list-id')"
						v-model="nickname"
						v-on:update="searchFieldChanged"
						placeholder="Никнейм"
					/>

					<datalist :id="$id('users-list-id')">
						<option v-for="person in filteredPersons" :key="person.id">{{ person.nickname }}</option>
					</datalist>

					<b-button
						class="mt-2"
						v-on:click="inviteInGroup"
						:disabled="!searchedUserId"
						:variant="searchedUserId ? 'outline-primary' : 'outline-secondary'"
					>
						Пригласить человека
					</b-button>
				</b-col>
			</b-row>

		</b-list-group-item>
	</div>
</template>
<script>
	import {mapActions} from 'vuex';
	import {searchPersons} from '../../services/persons.service';

	export default {
		name: 'group-list-item',
		props: {
			group: {},
		},

		data() {
			return {
				shown: null,
				name: this.group.name,
				purpose: this.group.purpose,
				nickname: '',
				persons: [],
			};
		},

		computed: {
			searchedUserId() {
				return (this.filteredPersons.find(x => x.nickname === this.nickname) || {}).id;
			},

			filteredPersons() {
				return this.persons.filter(x =>
					!(this.group.members || []).some(m => m.id === x.id)
					&& !(this.group.invites || []).some(m => m.id === x.id),
				);
			},
		},

		methods: {
			...mapActions('groups', [
				'upsertGroup', 'deleteGroup', 'loadGroupDetails', 'invitePersonInGroup', 'deletePersonFromGroup',
			]),

			async switchSpoiler() {
				if (this.shown === null) {
					await this.loadGroupDetails(this.group);
				}

				this.shown = !this.shown;
			},

			async saveGroup() {
				await this.upsertGroup({
					...this.group,
					name: this.name,
					purpose: this.purpose,
				});
			},

			async removeGroup() {
				await this.deleteGroup(this.group);
			},

			async searchFieldChanged() {
				if (this.nickname.length > 0) {
					this.persons = await searchPersons(this.nickname);
				} else {
					this.persons = [];
				}
			},

			async inviteInGroup() {
				await this.invitePersonInGroup({userId: this.searchedUserId, userNickname: this.nickname, group: this.group});
				this.persons = [];
				this.nickname = '';
			},
		},
	};
</script>